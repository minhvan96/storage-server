using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vnr.Storage.API.Infrastructure.Models;

namespace Vnr.Storage.API.Features.DecryptData.Commands
{
    public class DecryptMultipleFileCommandHandler : IRequestHandler<DecryptMultipleFileCommand, FileContentResultModel>
    {
        public Task<FileContentResultModel> Handle(DecryptMultipleFileCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}