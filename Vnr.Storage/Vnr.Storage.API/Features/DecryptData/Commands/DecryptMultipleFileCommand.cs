using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Vnr.Storage.API.Infrastructure.Models;

namespace Vnr.Storage.API.Features.DecryptData.Commands
{
    public class DecryptMultipleFileCommand : IRequest<FileContentResultModel>
    {
        public List<IFormFile> Files { get; set; }
    }
}