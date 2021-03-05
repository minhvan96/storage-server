//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using Vnr.Storage.API.Features.FileBrowser.Queries;
//using Vnr.Storage.API.Infrastructure;

//namespace Vnr.Storage.API.Features.FileBrowser
//{
//    [Route("api/[controller]")]
//    public class FileBrowserController : ApiControllerBase
//    {
//        public FileBrowserController(IMediator mediator) : base(mediator)
//        {
//        }

//        [HttpGet]
//        public Task<IActionResult> List(FullListQuery query)
//            => HandleRequest(query);
//    }
//}