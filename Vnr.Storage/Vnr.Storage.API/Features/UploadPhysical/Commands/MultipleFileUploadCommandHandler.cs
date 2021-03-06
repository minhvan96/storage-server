﻿using MediatR;
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
using Vnr.Storage.API.Features.UploadPhysical.Helpers;
using Vnr.Storage.API.Infrastructure.BaseResponse;
using Vnr.Storage.API.Infrastructure.Data;
using Vnr.Storage.API.Infrastructure.Data.Entities;
using Vnr.Storage.API.Infrastructure.Enums;
using Vnr.Storage.API.Infrastructure.Models;
using Vnr.Storage.API.Infrastructure.Utilities;
using Vnr.Storage.API.Infrastructure.Utilities.FileHelpers;
using Vnr.Storage.Security.Crypto.Symmetric;

namespace Vnr.Storage.API.Features.UploadPhysical.Commands
{
    public class MultipleFileUploadCommandHandler : IRequestHandler<MultipleFileUploadCommand, ResponseModel>
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly long _streamFileLimitSize;
        private readonly string[] _permittedExtensions;
        private readonly string _contentRootPath;
        private static readonly FormOptions _defaultFormOptions = new FormOptions();
        private readonly StorageContext _context;

        public MultipleFileUploadCommandHandler(IConfiguration configuration, IWebHostEnvironment env, IHttpContextAccessor accessor, StorageContext context)
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

        public async Task<ResponseModel> Handle(MultipleFileUploadCommand request, CancellationToken cancellationToken)
        {
            var errorModel = new FormFileErrorModel();

            if (!MultipartRequestHelper.IsMultipartContentType(_accessor.HttpContext.Request.ContentType))
            {
                errorModel.Errors.Add("File",
                    $"The request couldn't be processed (Error 1).");

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
                            _permittedExtensions, _streamFileLimitSize, ValidateExtension.Encrypt);

                        if (errorModel.Errors.Any())
                        {
                            return ResponseProvider.Ok(errorModel);
                        }
                        var fileName = FileHelpers.GetFileName(section.ContentDisposition);

                        var fileNameWithEncryptExtension = UploadFileHelper.GetFileNameWithEncryptExtension(fileName, request.EncryptAlg);
                        var uploadFileAbsolutePath = UploadFileHelper.GetUploadAbsolutePath(_contentRootPath, fileNameWithEncryptExtension, request.Archive);

                        await UploadFile(streamedFileContent, uploadFileAbsolutePath, request.EncryptAlg);
                    }
                }

                section = await reader.ReadNextSectionAsync(cancellationToken);
            }

            return ResponseProvider.Ok("Upload file successfully");
        }

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

        private async Task<bool> EncryptDataToFile(byte[] streamedFileContent, string absolutePath)
        {
            RijndaelManaged myRijndael = new RijndaelManaged();
            var rijndaeData = await _context.RijndaelKeys.FirstOrDefaultAsync();
            myRijndael.Key = Convert.FromBase64String(rijndaeData.Key);
            myRijndael.IV = Convert.FromBase64String(rijndaeData.IV);

            return SymmetricCrypto.EncryptDataAndSaveToFile(streamedFileContent, myRijndael.Key, myRijndael.IV, absolutePath, CryptoAlgorithm.Rijndael);
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