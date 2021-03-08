using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vnr.Storage.API.Infrastructure.BaseResponse;
using Vnr.Storage.API.Infrastructure.Enums;

namespace Vnr.Storage.API.Features.BufferedFileUploadPhysical.Commands
{
    public class BufferedMultipleFileUploadPhysicalCommand : IRequest<ResponseModel>
    {
        [Required]
        public List<IFormFile> Files { get; set; }

        [Required]
        public Infrastructure.Enums.Archive Archive { get; set; }
    }

    public class BufferedMultipleFileUploadPhysicalRequest
    {
        [Required]
        public List<IFormFile> Files { get; set; }

        [Required]
        public Infrastructure.Enums.Archive Archive { get; set; }
    }
}