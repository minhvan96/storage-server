using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Vnr.Storage.API.Infrastructure.BaseResponse;

namespace Vnr.Storage.API.Features.StreamedUploadPhysical.Commands
{
    public class StreamMultipleFileUploadCommand : IRequest<ResponseModel>
    {
        [Required]
        public IFormFileCollection Files { get; set; }

        [Required]
        public string Archive { get; set; }

        public bool Encrypt { get; set; }
    }
}