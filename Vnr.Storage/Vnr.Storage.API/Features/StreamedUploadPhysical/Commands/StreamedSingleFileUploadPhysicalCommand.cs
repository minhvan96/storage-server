using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Vnr.Storage.API.Infrastructure.BaseResponse;
using Vnr.Storage.API.Infrastructure.Enums;

namespace Vnr.Storage.API.Features.StreamedUploadPhysical.Commands
{
    public class StreamedSingleFileUploadPhysicalCommand : IRequest<ResponseModel>
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public Archive Archive { get; set; }
    }
}