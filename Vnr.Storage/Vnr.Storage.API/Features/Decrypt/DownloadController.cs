using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Vnr.Storage.API.Features.Decrypt.Queries;
using Vnr.Storage.API.Infrastructure;

namespace Vnr.Storage.API.Features.Decrypt
{
    [Route("api/download/archive")]
    public class DownloadController : ApiControllerBase
    {
        public DownloadController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("{CategoryName}/{FileName}")]
        public Task<IActionResult> Download(GetByFileNameQuery query)
            => HandleRequest(query);
    }
}