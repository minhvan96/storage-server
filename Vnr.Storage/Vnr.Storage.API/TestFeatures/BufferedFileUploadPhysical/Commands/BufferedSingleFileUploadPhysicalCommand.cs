using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Vnr.Storage.API.Infrastructure.BaseResponse;

namespace Vnr.Storage.API.Features.BufferedFileUploadPhysical.Commands
{
    public class BufferedSingleFileUploadPhysicalCommand : IRequest<ResponseModel>
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public Infrastructure.Enums.Archive Archive { get; set; }
    }

    public class BufferedSingleFileUploadPhysicalRequest
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public Infrastructure.Enums.Archive Archive { get; set; }
    }
}