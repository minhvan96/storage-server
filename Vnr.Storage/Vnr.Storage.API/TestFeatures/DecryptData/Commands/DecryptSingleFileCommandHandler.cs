//using MediatR;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.Features;
//using Microsoft.AspNetCore.WebUtilities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Net.Http.Headers;
//using System;
//using System.IO;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Threading;
//using System.Threading.Tasks;
//using Vnr.Storage.API.Configuration;
//using Vnr.Storage.API.Infrastructure.Data;
//using Vnr.Storage.API.Infrastructure.Models;
//using Vnr.Storage.API.Infrastructure.Utilities;
//using Vnr.Storage.API.Infrastructure.Utilities.FileHelpers;
//using Vnr.Storage.Security.Crypto.Symmetric;

//namespace Vnr.Storage.API.Features.DecryptData.Commands
//{
//    public class DecryptSingleFileCommandHandler : IRequestHandler<DecryptSingleFileCommand, FileContentResultModel>
//    {
//        private readonly IHttpContextAccessor _accessor;
//        private static readonly FormOptions _defaultFormOptions = new FormOptions();
//        private readonly string[] _permittedExtensions;
//        private readonly StorageContext _context;
//        private readonly long _streamFileLimitSize;

//        public DecryptSingleFileCommandHandler(IConfiguration configuration, IWebHostEnvironment env, IHttpContextAccessor accessor, StorageContext context)
//        {
//            var fileSizeLimitConfiguration = configuration.GetSection(nameof(FileSizeLimitConfiguration)).Get<FileSizeLimitConfiguration>();
//            _streamFileLimitSize = fileSizeLimitConfiguration.StreamFileSizeLimit;

//            var encryptedFileExtensionsConfiguration = configuration.GetSection(nameof(EncryptedFileExtensionsConfiguration)).Get<EncryptedFileExtensionsConfiguration>();
//            _permittedExtensions = encryptedFileExtensionsConfiguration.EncryptedFileExtensions;
//            _accessor = accessor;
//            _context = context;
//        }

//        public async Task<FileContentResultModel> Handle(DecryptSingleFileCommand request, CancellationToken cancellationToken)
//        {
//            var errorModel = new FormFileErrorModel();

//            var boundary = MultipartRequestHelper.GetBoundary(
//                            MediaTypeHeaderValue.Parse(_accessor.HttpContext.Request.ContentType),
//                            _defaultFormOptions.MultipartBoundaryLengthLimit);
//            var reader = new MultipartReader(boundary, _accessor.HttpContext.Request.Body);
//            var section = await reader.ReadNextSectionAsync(cancellationToken);

//            while (section != null)
//            {
//                var hasContentDispositionHeader =
//                    ContentDispositionHeaderValue.TryParse(
//                        section.ContentDisposition, out var contentDisposition);

//                if (hasContentDispositionHeader)
//                {
//                    var response = new FileContentResultModel();

//                    if (!MultipartRequestHelper
//                        .HasFileContentDisposition(contentDisposition))
//                    {
//                        errorModel.Errors.Add("File", $"The request couldn't be processed (Error 2).");

//                        return response;
//                    }
//                    else
//                    {
//                        var streamedFileContent = await FileHelpers.ProcessStreamedFile(
//                            section, contentDisposition, errorModel,
//                            _permittedExtensions, _streamFileLimitSize, Infrastructure.Enums.ValidateExtension.Decrypt);

//                        if (errorModel.Errors.Any())
//                        {
//                            return response;
//                        }

//                        response.StreamData = await DecryptFileContentToStream(streamedFileContent);
//                        response.FileName = Path.GetFileNameWithoutExtension(request.File.FileName);

//                        return response;
//                    }
//                }

//                section = await reader.ReadNextSectionAsync(cancellationToken);
//            }
//            return new FileContentResultModel();
//        }

//        private async Task<Stream> DecryptFileContentToStream(byte[] data)
//        {
//            RijndaelManaged myRijndael = new RijndaelManaged();

//            var rijndaeData = await _context.RijndaelKeys.FirstOrDefaultAsync();
//            myRijndael.Key = Convert.FromBase64String(rijndaeData.Key);
//            myRijndael.IV = Convert.FromBase64String(rijndaeData.IV);

//            return SymmetricCrypto.DecryptDataToStream(data, myRijndael.Key, myRijndael.IV, CryptoAlgorithm.Rijndael);
//        }
//    }
//}