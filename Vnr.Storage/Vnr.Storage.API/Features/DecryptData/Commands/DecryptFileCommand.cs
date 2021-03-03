using MediatR;
using Microsoft.AspNetCore.Http;
using Vnr.Storage.API.Infrastructure.BaseResponse;
using Vnr.Storage.API.Infrastructure.Models;

namespace Vnr.Storage.API.Features.DecryptData.Commands
{
    public class DecryptFileCommand : IRequest<FileContentResultModel>
    {
        public IFormFile File { get; set; }
    }
}