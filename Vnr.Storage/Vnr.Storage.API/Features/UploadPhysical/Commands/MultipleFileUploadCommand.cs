using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
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

        public EncryptAlg EncryptAlg { get; set; }
    }

    public class MultipleUploadResponse
    {
        public List<string> Urls { get; set; }
    }
}