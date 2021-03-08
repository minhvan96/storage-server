using MediatR;
using Vnr.Storage.API.Infrastructure.BaseResponse;
using Vnr.Storage.API.Infrastructure.Common.Query;
using Vnr.Storage.API.Infrastructure.Data.Entities;
using Vnr.Storage.API.Infrastructure.Platform.Common.Query;

namespace Vnr.Storage.API.Features.FileBrowser.Queries
{
    public class FullListQuery : PageQuery, IRequest<ResponseModel<PagedList<EncryptedFile>>>
    {
    }
}