using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vnr.Storage.API.Features.BufferedFileUploadPhysical.Helpers;
using Vnr.Storage.API.Infrastructure.BaseResponse;
using Vnr.Storage.API.Infrastructure.Configuration;
using Vnr.Storage.API.Infrastructure.Models;
using Vnr.Storage.API.Infrastructure.Utilities;

namespace Vnr.Storage.API.Features.BufferedFileUploadPhysical.Commands
{
    public class BufferedSingleFileUploadPhysicalCommandHandler : IRequestHandler<BufferedSingleFileUploadPhysicalCommand, ResponseModel>
    {
        private readonly long _defaultFileSizeLimit;
        private readonly string[] _permittedExtensions = { ".txt", ".pdf", ".docx" };
        private readonly string _contentRootPath;
        private FormFileErrorModel _errorModel;

        public BufferedSingleFileUploadPhysicalCommandHandler(IConfiguration configuration, IWebHostEnvironment env)
        {
            var fileSizeLimitConfiguration = configuration.GetSection(nameof(FileSizeLimitConfiguration)).Get<FileSizeLimitConfiguration>();
            _defaultFileSizeLimit = fileSizeLimitConfiguration.DefaultFileSizeLimit;
            _contentRootPath = env.ContentRootPath;
            _errorModel = new FormFileErrorModel();
        }

        public async Task<ResponseModel> Handle(BufferedSingleFileUploadPhysicalCommand request, CancellationToken cancellationToken)
        {
            var formFileContent =
                await FileHelpers.ProcessFormFile<BufferedSingleFileUploadPhysical>(
                    request.File, _errorModel, _permittedExtensions,
                    _defaultFileSizeLimit);

            if (_errorModel.Errors.Any())
            {
                return ResponseProvider.Ok(_errorModel);
            }

            var filePath = UploadFileHelper.UploadFileLocation(_contentRootPath, request.File.FileName, request.Archive);

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