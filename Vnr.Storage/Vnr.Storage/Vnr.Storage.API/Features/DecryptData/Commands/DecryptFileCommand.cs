using MediatR;
using Microsoft.AspNetCore.Http;
using Vnr.Storage.API.Infrastructure.BaseResponse;

namespace Vnr.Storage.API.Features.DecryptData.Commands
{
    public class DecryptFileCommand : IRequest<ResponseModel>
    {
        public IFormFile File { get; set; }
    }
}