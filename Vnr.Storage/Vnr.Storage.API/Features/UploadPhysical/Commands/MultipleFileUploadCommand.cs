using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Vnr.Storage.API.Infrastructure.BaseResponse;
using Vnr.Storage.API.Infrastructure.Enums;

namespace Vnr.Storage.API.Features.UploadPhysical.Commands
{
    public class MultipleFileUploadCommand : IRequest<ResponseModel>
    {
        [Required]
        public IFormFileCollection Files { get; set; }

        [Required]
        public Archive Archive { get; set; }

        public bool Encrypt { get; set; }
    }
}