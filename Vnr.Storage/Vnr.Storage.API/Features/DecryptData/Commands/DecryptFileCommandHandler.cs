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
using Vnr.Storage.API.Infrastructure.BaseResponse;
using Vnr.Storage.API.Infrastructure.Configuration;
using Vnr.Storage.API.Infrastructure.Crypto;
using Vnr.Storage.API.Infrastructure.Data;
using Vnr.Storage.API.Infrastructure.Models;
using Vnr.Storage.API.Infrastructure.Utilities;
using Vnr.Storage.API.Infrastructure.Utilities.FileHelpers;

namespace Vnr.Storage.API.Features.DecryptData.Commands
{
    public class DecryptFileCommandHandler : IRequestHandler<DecryptFileCommand, ResponseModel>
    {
        private readonly IHttpContextAccessor _accessor;
        private static readonly FormOptions _defaultFormOptions = new FormOptions();
        private readonly string[] _permittedExtensions = { ".vnresource" };
        private readonly StorageContext _context;
        private readonly long _streamFileLimitSize;

        public DecryptFileCommandHandler(IConfiguration configuration, IWebHostEnvironment env, IHttpContextAccessor accessor, StorageContext context)
        {
            var fileSizeLimitConfiguration = configuration.GetSection(nameof(FileSizeLimitConfiguration)).Get<FileSizeLimitConfiguration>();
            _streamFileLimitSize = fileSizeLimitConfiguration.StreamFileSizeLimit;
            _accessor = accessor;
            _context = context;
        }

        public async Task<ResponseModel> Handle(DecryptFileCommand request, CancellationToken cancellationToken)
        {
            var errorModel = new FormFileErrorModel();

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
                            _permittedExtensions, _streamFileLimitSize, Infrastructure.Enums.ValidateExtension.Decrypt);

                        if (errorModel.Errors.Any())
                        {
                            return ResponseProvider.Ok(errorModel);
                        }

                        RijndaelManaged myRijndael = new RijndaelManaged();
                        var rijndaeData = await _context.RijndaelKeys.FirstOrDefaultAsync(cancellationToken);
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

        public async Task DecryptFileContent(byte[] content)
        {
            RijndaelManaged myRijndael = new RijndaelManaged();

            var rijndaeData = await _context.RijndaelKeys.FirstOrDefaultAsync();
            myRijndael.Key = Convert.FromBase64String(rijndaeData.Key);
            myRijndael.IV = Convert.FromBase64String(rijndaeData.IV);

            var decryptedFileContent = RijndaelCrypto.DecryptStringFromBytes(content, myRijndael.Key, myRijndael.IV);
        }
    }
}