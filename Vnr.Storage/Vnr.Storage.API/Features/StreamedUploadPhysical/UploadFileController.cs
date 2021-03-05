using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Vnr.Storage.API.Features.StreamedUploadPhysical.Commands;
using Vnr.Storage.API.Infrastructure;

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
        public Task<IActionResult> StreamedSingleFileUploadPhysical(StreamedSingleFileUploadPhysicalCommand command)
            => HandleRequest(command);
    }
}