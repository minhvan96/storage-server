using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
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
    public class BufferedMultipleFileUploadPhysicalCommandHandler : IRequestHandler<BufferedMultipleFileUploadPhysicalCommand, ResponseModel>
    {
        private readonly long _defaultFileSizeLimit;
        private readonly string[] _permittedExtensions;
        private readonly string _contentRootPath;

        public BufferedMultipleFileUploadPhysicalCommandHandler(IConfiguration configuration, IWebHostEnvironment env)
        {
            var fileSizeLimitConfiguration = configuration.GetSection(nameof(FileSizeLimitConfiguration)).Get<FileSizeLimitConfiguration>();
            var bufferedFileUploadPhysicalPermittedExtensionsConfiguration = configuration
                .GetSection(nameof(BufferedFileUploadPhysicalPermittedExtensionsConfiguration))
                .Get<BufferedFileUploadPhysicalPermittedExtensionsConfiguration>();
            _permittedExtensions = bufferedFileUploadPhysicalPermittedExtensionsConfiguration.MultipleFileUploadPermittedExtensions;
            _defaultFileSizeLimit = fileSizeLimitConfiguration.DefaultFileSizeLimit;
            _contentRootPath = env.ContentRootPath;
        }

        public async Task<ResponseModel> Handle(BufferedMultipleFileUploadPhysicalCommand request, CancellationToken cancellationToken)
        {
            var errorModel = new FormFileErrorModel();
            foreach (var formFile in request.Files)
            {
                var formFileContent =
                    await FileHelpers
                        .ProcessFormFile<BufferedMultipleFileUploadPhysical>(
                            formFile, errorModel, _permittedExtensions,
                            _defaultFileSizeLimit);

                if (errorModel.Errors.Any())
                {
                    return ResponseProvider.Ok(errorModel);
                }

                // For the file name of the uploaded file stored
                // server-side, use Path.GetRandomFileName to generate a safe
                // random file name.

                var filePath = UploadFileHelper.GetUploadAbsolutePath(_contentRootPath, formFile.FileName, request.Archive);
                //var trustedFileNameForFileStorage = Path.GetRandomFileName();
                //var filePath = Path.Combine(
                //    _targetFilePath, trustedFileNameForFileStorage);

                // **WARNING!**
                // In the following example, the file is saved without
                // scanning the file's contents. In most production
                // scenarios, an anti-virus/anti-malware scanner API
                // is used on the file before making the file available
                // for download or for use by other systems.
                // For more information, see the topic that accompanies
                // this sample.

                using (var fileStream = File.Create(filePath))
                {
                    await fileStream.WriteAsync(formFileContent, cancellationToken);

                    // To work directly with the FormFiles, use the following
                    // instead:
                    //await formFile.CopyToAsync(fileStream);
                }
            }
            return ResponseProvider.Ok("Upload file successful");
        }

        public class BufferedMultipleFileUploadPhysical
        {
            [Required]
            [Display(Name = "File")]
            public List<IFormFile> Files { get; set; }

            [Display(Name = "Note")]
            [StringLength(50, MinimumLength = 0)]
            public string Note { get; set; }
        }
    }
}