using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;
using Vnr.Storage.API.Infrastructure.BaseResponse;
using Vnr.Storage.API.Infrastructure.Models;
using Vnr.Storage.API.Infrastructure.Utilities.FileHelpers;

namespace Vnr.Storage.API.Infrastructure
{
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class ApiControllerBase : Controller
    {
        private readonly IMediator _mediator;

        protected ApiControllerBase(IMediator mediator)
            => _mediator = mediator;

        protected async Task<IActionResult> HandleRequest<T>(IRequest<ResponseModel<T>> request)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseProvider.BadRequest(ModelState);
                return new JsonResult(errorResponse)
                {
                    StatusCode = (int)errorResponse.StatusCode
                };
            }

            var result = await _mediator.Send(request);

            return new JsonResult(result)
            {
                StatusCode = (int)result.StatusCode
            };
        }

        protected async Task<IActionResult> HandleRequest(IRequest<ResponseModel<FileContentResultModel>> request)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseProvider.BadRequest(ModelState);
                return new JsonResult(errorResponse)
                {
                    StatusCode = (int)errorResponse.StatusCode
                };
            }

            var result = await _mediator.Send(request);
            if (result.Successed)
            {
                var contentType = FileHelpers.GetMIMEType(result.Data.FileName);
                return File(result.Data.StreamData, contentType, result.Data.FileName);
            }
            return new JsonResult(result)
            {
                StatusCode = (int)result.StatusCode
            };
        }

        protected async Task<IActionResult> HandleRequest(IRequest<ResponseModel> request)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseProvider.BadRequest(ModelState);
                return new JsonResult(errorResponse)
                {
                    StatusCode = (int)errorResponse.StatusCode
                };
            }

            var result = await _mediator.Send(request);

            return new JsonResult(result)
            {
                StatusCode = (int)result.StatusCode
            };
        }
    }
}