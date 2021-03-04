using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Vnr.Storage.API.Infrastructure.BaseResponse;
using Vnr.Storage.API.Infrastructure.Data;
using Vnr.Storage.API.Infrastructure.Data.Entities;
using Vnr.Storage.API.Infrastructure.Platform.Common.Query;

namespace Vnr.Storage.API.Features.FileBrowser.Queries
{
    public class FullListQueryHandler : IRequestHandler<FullListQuery, ResponseModel<PagedList<EncryptedFile>>>
    {
        private readonly StorageContext _context;

        public FullListQueryHandler(StorageContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<PagedList<EncryptedFile>>> Handle(FullListQuery request, CancellationToken cancellationToken)
        {
            var encryptedFiles = _context.EncryptedFiles
                .AsNoTracking();

            var response = await PagedList<EncryptedFile>.Create(encryptedFiles, request.Page, request.PageSize);
            return ResponseProvider.Ok(response);
        }
    }
}