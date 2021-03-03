using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Vnr.Storage.API.Features.BufferedFileUploadPhysical.Helpers;
using Vnr.Storage.API.Infrastructure.BaseResponse;
using Vnr.Storage.API.Infrastructure.Configuration;
using Vnr.Storage.API.Infrastructure.Crypto;
using Vnr.Storage.API.Infrastructure.Data;
using Vnr.Storage.API.Infrastructure.Models;
using Vnr.Storage.API.Infrastructure.Utilities;

namespace Vnr.Storage.API.Features.DecryptData.Commands
{
    public class DecryptFileCommandHandler : IRequestHandler<DecryptFileCommand, ResponseModel>
    {
        private readonly IHttpContextAccessor _accessor;
        private static readonly FormOptions _defaultFormOptions = new FormOptions();
        private readonly string[] _permittedExtensions = { ".txt", ".pdf", ".docx", "msi" };
        private readonly StorageContext _context;
        private FormFileErrorModel _errorModel;
        private readonly long _streamFileLimitSize;

        public DecryptFileCommandHandler(IConfiguration configuration, IWebHostEnvironment env, IHttpContextAccessor accessor, StorageContext context)
        {
            var fileSizeLimitConfiguration = configuration.GetSection(nameof(FileSizeLimitConfiguration)).Get<FileSizeLimitConfiguration>();
            _streamFileLimitSize = fileSizeLimitConfiguration.StreamFileSizeLimit;
            _accessor = accessor;
            _context = context;
            _errorModel = new FormFileErrorModel();
        }

        public async Task<ResponseModel> Handle(DecryptFileCommand request, CancellationToken cancellationToken)
        {
            if (!MultipartRequestHelper.IsMultipartContentType(_accessor.HttpContext.Request.ContentType))
            {
                _errorModel.Errors.Add("File",
                    $"The request couldn't be processed (Error 1).");
                // Log error

                return ResponseProvider.Ok(_errorModel);
            }
            var boundary = MultipartRequestHelper.GetBoundary(
                            MediaTypeHeaderValue.Parse(_accessor.HttpContext.Request.ContentType),
                            _defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, _accessor.HttpContext.Request.Body);
            var section = await reader.ReadNextSectionAsync(cancellationToken);

            while (section != null)
            {
                var hasContentDispositionHeader =
                    ContentDispositionHeaderValue.TryParse(
                        section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (!MultipartRequestHelper
                        .HasFileContentDisposition(contentDisposition))
                    {
                        _errorModel.Errors.Add("File", $"The request couldn't be processed (Error 2).");

                        return ResponseProvider.Ok(_errorModel);
                    }
                    else
                    {
                        var streamedFileContent = await FileHelpers.ProcessStreamedFile(
                            section, contentDisposition, _errorModel,
                            _permittedExtensions, _streamFileLimitSize, Infrastructure.Enums.ValidateExtension.Decrypt);

                        if (_errorModel.Errors.Any())
                        {
                            return ResponseProvider.Ok(_errorModel);
                        }

                        RijndaelManaged myRijndael = new RijndaelManaged();
                        var rijndaeData = await _context.RijndaelKeys.FirstOrDefaultAsync();
                        myRijndael.Key = Convert.FromBase64String(rijndaeData.Key);
                        myRijndael.IV = Convert.FromBase64String(rijndaeData.IV);

                        var encryptedFileContent = RijndaelCrypto.DecryptStringFromBytes(streamedFileContent, myRijndael.Key, myRijndael.IV);
                        var testData = System.Text.Encoding.UTF8.GetString(encryptedFileContent);

                        return ResponseProvider.Ok(testData);
                    }
                }

                section = await reader.ReadNextSectionAsync(cancellationToken);
            }
            return ResponseProvider.Ok();
        }
    }
}