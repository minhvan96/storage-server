using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Vnr.Storage.API.Infrastructure.BaseResponse;
using Vnr.Storage.API.Infrastructure.Data.Entities;

namespace Vnr.Storage.API.Features.FileBrowser.Queries
{
    public class GetQueryHandler : IRequestHandler<GetQuery, ResponseModel<EncryptedFile>>
    {
        public Task<ResponseModel<EncryptedFile>> Handle(GetQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}