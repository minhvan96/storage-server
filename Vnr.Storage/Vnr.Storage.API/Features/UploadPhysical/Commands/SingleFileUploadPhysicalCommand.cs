using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Vnr.Storage.API.Infrastructure.BaseResponse;
using Vnr.Storage.API.Infrastructure.Enums;

namespace Vnr.Storage.API.Features.UploadPhysical.Commands
{
    public class SingleFileUploadPhysicalCommand : IRequest<ResponseModel<SingleUploadResponse>>
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public Archive Archive { get; set; }

        public EncryptAlg EncryptAlg { get; set; }
    }

    public class SingleUploadResponse
    {
        public string Url { get; set; }
    }
}