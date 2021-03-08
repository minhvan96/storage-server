//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using Vnr.Storage.API.Features.DecryptData.Commands;
//using Vnr.Storage.API.Features.DecryptData.Queries;
//using Vnr.Storage.API.Infrastructure;

//namespace Vnr.Storage.API.Features.DecryptData
//{
//    [Consumes("multipart/form-data")]
//    [Route("api/archive/download")]
//    public class DownloadFileController : ApiControllerBase
//    {
//        public DownloadFileController(IMediator mediator) : base(mediator)
//        {
//        }

//        [HttpGet]
//        public Task<IActionResult> DecryptFile(DecryptSingleFileByIdQuery query)
//            => HandleRequest(query);

//        [HttpPost]
//        public Task<IActionResult> DecryptFile(DecryptSingleFileCommand command)
//            => HandleRequest(command);
//    }
//}