using MediatR;
using System;
using Vnr.Storage.API.Infrastructure.BaseResponse;
using Vnr.Storage.API.Infrastructure.Data.Entities;

namespace Vnr.Storage.API.Features.FileBrowser.Queries
{
    public class GetQuery : IRequest<ResponseModel<EncryptedFile>>
    {
        public Guid Id { get; set; }
    }
}