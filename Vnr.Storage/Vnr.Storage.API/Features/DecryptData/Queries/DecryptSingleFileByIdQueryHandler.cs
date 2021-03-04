using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vnr.Storage.API.Infrastructure.Data;
using Vnr.Storage.API.Infrastructure.Models;

namespace Vnr.Storage.API.Features.DecryptData.Queries
{
    public class DecryptSingleFileByIdQueryHandler : IRequestHandler<DecryptSingleFileByIdQuery, FileContentResultModel>
    {
        private readonly IHttpContextAccessor _accessor;
        private static readonly FormOptions _defaultFormOptions = new FormOptions();
        private readonly StorageContext _context;

        public DecryptSingleFileByIdQueryHandler(IHttpContextAccessor accessor, StorageContext context)
        {
            _accessor = accessor;
            _context = context;
        }

        public Task<FileContentResultModel> Handle(DecryptSingleFileByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}