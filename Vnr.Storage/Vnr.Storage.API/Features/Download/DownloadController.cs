using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Vnr.Storage.API.Features.Download.Queries;
using Vnr.Storage.API.Infrastructure;

namespace Vnr.Storage.API.Features.Download
{
    [Route("api/download/archive")]
    [Authorize]
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