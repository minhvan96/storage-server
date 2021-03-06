﻿using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vnr.Storage.API.Configuration;
using Vnr.Storage.API.Features.BufferedFileUploadPhysical.Helpers;
using Vnr.Storage.API.Infrastructure.BaseResponse;
using Vnr.Storage.API.Infrastructure.Models;
using Vnr.Storage.API.Infrastructure.Utilities.FileHelpers;

namespace Vnr.Storage.API.Features.BufferedFileUploadPhysical.Commands
{
    public class BufferedSingleFileUploadPhysicalCommandHandler : IRequestHandler<BufferedSingleFileUploadPhysicalCommand, ResponseModel>
    {
        private readonly long _defaultFileSizeLimit;
        private readonly string[] _permittedExtensions;
        private readonly string _contentRootPath;

        public BufferedSingleFileUploadPhysicalCommandHandler(IConfiguration configuration, IWebHostEnvironment env)
        {
            var fileSizeLimitConfiguration = configuration.GetSection(nameof(FileSizeLimitConfiguration)).Get<FileSizeLimitConfiguration>();
            var bufferedFileUploadPhysicalPermittedExtensionsConfiguration = configuration
               .GetSection(nameof(BufferedFileUploadPhysicalPermittedExtensionsConfiguration))
               .Get<BufferedFileUploadPhysicalPermittedExtensionsConfiguration>();
            _permittedExtensions = bufferedFileUploadPhysicalPermittedExtensionsConfiguration.SingleFileUploadPermittedExtensions;
            _defaultFileSizeLimit = fileSizeLimitConfiguration.DefaultFileSizeLimit;
            _contentRootPath = env.ContentRootPath;
        }

        public async Task<ResponseModel> Handle(BufferedSingleFileUploadPhysicalCommand request, CancellationToken cancellationToken)
        {
            var errorModel = new FormFileErrorModel();
            var formFileContent =
                await FileHelpers.ProcessFormFile<BufferedSingleFileUploadPhysical>(
                    request.File, errorModel, _permittedExtensions,
                    _defaultFileSizeLimit);

            if (errorModel.Errors.Any())
            {
                return ResponseProvider.Ok(errorModel);
            }

            var filePath = UploadFileHelper.GetUploadAbsolutePath(_contentRootPath, request.File.FileName, request.Archive);

            using (var fileStream = File.Create(filePath))
            {
                await fileStream.WriteAsync(formFileContent, cancellationToken);
            }
            return ResponseProvider.Ok("Upload file successful");
        }

        public class BufferedSingleFileUploadPhysical
        {
            [Required]
            [Display(Name = "File")]
            public IFormFile FormFile { get; set; }

            [Display(Name = "Note")]
            [StringLength(50, MinimumLength = 0)]
            public string Note { get; set; }
        }
    }
}