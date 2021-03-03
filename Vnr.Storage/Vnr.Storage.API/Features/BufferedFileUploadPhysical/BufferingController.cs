using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Vnr.Storage.API.Features.BufferedFileUploadPhysical.Commands;
using Vnr.Storage.API.Infrastructure;

namespace Vnr.Storage.API.Features.BufferedFileUploadPhysical
{
    [Consumes("multipart/form-data")]
    [Route("api/[controller]")]
    public class BufferingController : ApiControllerBase
    {
        public BufferingController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("multiple-upload")]
        public Task<IActionResult> BufferedMultipleFileUploadPhysical(BufferedMultipleFileUploadPhysicalCommand command)
            => HandleRequest(command);

        [HttpPost("single-upload")]
        public Task<IActionResult> BufferedSingleFileUploadPhysical(BufferedSingleFileUploadPhysicalCommand command)
            => HandleRequest(command);
    }
}