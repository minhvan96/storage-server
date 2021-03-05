using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Vnr.Storage.API.Configuration;
using Vnr.Storage.API.Features.BufferedFileUploadPhysical.Helpers;
using Vnr.Storage.API.Infrastructure.BaseResponse;
using Vnr.Storage.API.Infrastructure.Crypto.RijndaelCrypto;
using Vnr.Storage.API.Infrastructure.Data;
using Vnr.Storage.API.Infrastructure.Data.Entities;
using Vnr.Storage.API.Infrastructure.Models;
using Vnr.Storage.API.Infrastructure.Utilities;
using Vnr.Storage.API.Infrastructure.Utilities.FileHelpers;

namespace Vnr.Storage.API.Features.StreamedUploadPhysical.Commands
{
    public class StreamedSingleFileUploadPhysicalCommandHandler : IRequestHandler<StreamedSingleFileUploadPhysicalCommand, ResponseModel>
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly long _streamFileLimitSize;
        private readonly string[] _permittedExtensions;
        private readonly string _contentRootPath;
        private static readonly FormOptions _defaultFormOptions = new FormOptions();
        private readonly StorageContext _context;

        public StreamedSingleFileUploadPhysicalCommandHandler(IConfiguration configuration, IWebHostEnvironment env, IHttpContextAccessor accessor, StorageContext context)
        {
            var fileSizeLimitConfiguration = configuration.GetSection(nameof(FileSizeLimitConfiguration)).Get<FileSizeLimitConfiguration>();
            _streamFileLimitSize = fileSizeLimitConfiguration.StreamFileSizeLimit;
            var streamedFileUploadPhysicalPermittedExtensionsConfiguration = configuration
               .GetSection(nameof(StreamedFileUploadPhysicalPermittedExtensionsConfiguration))
               .Get<StreamedFileUploadPhysicalPermittedExtensionsConfiguration>();
            _permittedExtensions = streamedFileUploadPhysicalPermittedExtensionsConfiguration.SingleFileUploadPermittedExtensions;
            _contentRootPath = env.ContentRootPath;
            _accessor = accessor;
            _context = context;
        }

        public async Task<ResponseModel> Handle(StreamedSingleFileUploadPhysicalCommand request, CancellationToken cancellationToken)
        {
            var errorModel = new FormFileErrorModel();

            if (!MultipartRequestHelper.IsMultipartContentType(_accessor.HttpContext.Request.ContentType))
            {
                errorModel.Errors.Add("File",
                    $"The request couldn't be processed (Error 1).");
                // Log error

                return ResponseProvider.Ok(errorModel);
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
                        errorModel.Errors.Add("File", $"The request couldn't be processed (Error 2).");

                        return ResponseProvider.Ok(errorModel);
                    }
                    else
                    {
                        var streamedFileContent = await FileHelpers.ProcessStreamedFile(
                            section, contentDisposition, errorModel,
                            _permittedExtensions, _streamFileLimitSize, Infrastructure.Enums.ValidateExtension.Encrypt);

                        if (errorModel.Errors.Any())
                        {
                            return ResponseProvider.Ok(errorModel);
                        }

                        var uploadFileAbsolutePath = UploadFileHelper.GetUploadAbsolutePath(_contentRootPath, request.File.FileName, request.Archive);
                        var uploadfileRelativePath = UploadFileHelper.GetUploadRelativePath(request.File.FileName, request.Archive);
                        var finalUploadFileAbsolutePath = uploadFileAbsolutePath + ".vnresource";
                        var finalUploadFileRelativePath = uploadfileRelativePath + ".vnresource";

                        await EncryptDataToFile(streamedFileContent, finalUploadFileAbsolutePath);

                        await UploadFilePathToDatabase(request.File.FileName, finalUploadFileRelativePath, finalUploadFileAbsolutePath);
                    }
                }

                section = await reader.ReadNextSectionAsync(cancellationToken);
            }
            return ResponseProvider.Ok("Upload file successful");
        }

        private async Task<bool> EncryptDataToFile(byte[] streamedFileContent, string absolutePath)
        {
            RijndaelManaged myRijndael = new RijndaelManaged();
            var rijndaeData = await _context.RijndaelKeys.FirstOrDefaultAsync();
            myRijndael.Key = Convert.FromBase64String(rijndaeData.Key);
            myRijndael.IV = Convert.FromBase64String(rijndaeData.IV);

            return Vnr.Storage.Security.Crypto.RijndaelCrypto.RijndaelCrypto.EncryptDataAndSaveToFile(streamedFileContent, myRijndael.Key, myRijndael.IV, absolutePath);
        }

        public async Task UploadFilePathToDatabase(string fileName, string path, string fullPath)
        {
            var encryptedFile = new EncryptedFile
            {
                FileName = fileName,
                Path = path,
                FullPath = fullPath
            };
            await _context.AddAsync(encryptedFile);
            await _context.SaveChangesAsync();
        }
    }
}