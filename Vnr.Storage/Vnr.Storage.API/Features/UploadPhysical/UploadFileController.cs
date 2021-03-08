using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Vnr.Storage.API.Features.UploadPhysical.Commands;
using Vnr.Storage.API.Infrastructure;
using Vnr.Storage.API.Infrastructure.Filters;

namespace Vnr.Storage.API.Features.StreamedUploadPhysical
{
    [Consumes("multipart/form-data")]
    [Route("api/archive/upload")]
    public class UploadFileController : ApiControllerBase
    {
        public UploadFileController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public Task<IActionResult> SingleFileUploadPhysical(SingleFileUploadPhysicalCommand command)
            => HandleRequest(command);

        [HttpPost("multiple")]
        [DisableFormValueModelBinding]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public Task<IActionResult> MultipleFileUploadPhysical(MultipleFileUploadCommand command)
            => HandleRequest(command);
    }
}