using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vnr.Storage.API.Configuration;
using Vnr.Storage.API.Features.UploadPhysical.Helpers;
using Vnr.Storage.API.Infrastructure.BaseResponse;
using Vnr.Storage.API.Infrastructure.Data;
using Vnr.Storage.API.Infrastructure.Enums;
using Vnr.Storage.API.Infrastructure.Models;
using Vnr.Storage.API.Infrastructure.Utilities;
using Vnr.Storage.API.Infrastructure.Utilities.FileHelpers;
using Vnr.Storage.Security.Crypto.Symmetric;

namespace Vnr.Storage.API.Features.UploadPhysical.Commands
{
    public class SingleFileUploadPhysicalCommandHandler : IRequestHandler<SingleFileUploadPhysicalCommand, ResponseModel<SingleUploadResponse>>
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly long _streamFileLimitSize;
        private readonly string[] _permittedExtensions;
        private readonly string _contentRootPath;
        private static readonly FormOptions _defaultFormOptions = new FormOptions();
        private readonly StorageContext _context;

        public SingleFileUploadPhysicalCommandHandler(IConfiguration configuration, IWebHostEnvironment env, IHttpContextAccessor accessor, StorageContext context)
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

        public async Task<ResponseModel<SingleUploadResponse>> Handle(SingleFileUploadPhysicalCommand request, CancellationToken cancellationToken)
        {
            var swTotalEncrypt = new Stopwatch();
            swTotalEncrypt.Start();
            var errorModel = new FormFileErrorModel();

            var multiPartContentTypeValidation = MultiPartContentTypeValidation(_accessor.HttpContext.Request.ContentType)
                .Select(x => x.ErrorMessage)
                .ToArray();
            if (multiPartContentTypeValidation.Any())
                return ResponseProvider.BadRequest<SingleUploadResponse>(multiPartContentTypeValidation);

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
                    var hasFileContentDispositionValidation = HasFileContentDispositionValidation(contentDisposition)
                        .Select(x => x.ErrorMessage)
                        .ToArray();
                    if (hasFileContentDispositionValidation.Any())
                        return ResponseProvider.BadRequest<SingleUploadResponse>(hasFileContentDispositionValidation);
                    else
                    {
                        var streamedFileContent = await FileHelpers.ProcessStreamedFile(
                            section, contentDisposition, errorModel,
                            _permittedExtensions, _streamFileLimitSize, ValidateExtension.Encrypt);

                        if (errorModel.Errors.Any())
                        {
                            //return ResponseProvider.BadRequest<SingleUploadResponse>(errorModel.Errors);
                            return ResponseProvider.Ok(new SingleUploadResponse());
                        }
                        var fileNameWithEncryptExtension = UploadFileHelper.GetFileNameWithEncryptExtension(request.File.FileName, request.EncryptAlg);
                        var uploadFileAbsolutePath = UploadFileHelper.GetUploadAbsolutePath(_contentRootPath, fileNameWithEncryptExtension, request.Archive);

                        await UploadFile(streamedFileContent, uploadFileAbsolutePath, request.EncryptAlg);
                    }
                }

                section = await reader.ReadNextSectionAsync(cancellationToken);
            }
            swTotalEncrypt.Stop();

            var requestScheme = _accessor.HttpContext.Request.Scheme;
            var domain = _accessor.HttpContext.Request.Host.Value;
            var url = Path.Combine(requestScheme, domain, "Archive", request.Archive.ToString(), request.File.FileName);

            Console.Write($"File length: {request.File.Length / 1024f / 1024f} MB");
            Console.WriteLine($"Encrypt time: {swTotalEncrypt.ElapsedMilliseconds}");

            return ResponseProvider.Ok(new SingleUploadResponse { Url = url });
        }

        #region Validation

        private static IEnumerable<ValidationResult> MultiPartContentTypeValidation(string contentType)
        {
            if (!MultipartRequestHelper.IsMultipartContentType(contentType))
                yield return new ValidationResult($"The request couldn't be processed (Error 2).", new[] { "File" });
        }

        private static IEnumerable<ValidationResult> HasFileContentDispositionValidation(ContentDispositionHeaderValue contentDisposition)
        {
            if (!MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                yield return new ValidationResult($"The request couldn't be processed (Error 2).", new[] { "File" });
        }

        #endregion Validation

        private async Task<bool> UploadFile(byte[] streamedFileContent, string absolutePath, EncryptAlg encryptAlg)
        {
            if (encryptAlg == EncryptAlg.None)
                return await FileHelpers.UploadFile(streamedFileContent, absolutePath);
            else
                return await EncryptDataToFile(streamedFileContent, absolutePath, encryptAlg);
        }

        private async Task<bool> EncryptDataToFile(byte[] streamedFileContent, string absolutePath, EncryptAlg encryptAlg)
        {
            if (encryptAlg == EncryptAlg.AES)
            {
                var aesData = await _context.AesKeys.FirstOrDefaultAsync();
                return SymmetricCrypto.EncryptDataAndSaveToFile(streamedFileContent, aesData.Key, aesData.IV, absolutePath, CryptoAlgorithm.Aes);
            }
            else
            {
                var rijndaeData = await _context.RijndaelKeys.FirstOrDefaultAsync();
                byte[] key = Convert.FromBase64String(rijndaeData.Key);
                byte[] IV = Convert.FromBase64String(rijndaeData.IV);
                return SymmetricCrypto.EncryptDataAndSaveToFile(streamedFileContent, key, IV, absolutePath);
            }
        }
    }
}