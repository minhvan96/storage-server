using MediatR;
using System;
using Vnr.Storage.API.Infrastructure.Models;

namespace Vnr.Storage.API.Features.DecryptData.Queries
{
    public class DecryptSingleFileByIdQuery : IRequest<FileContentResultModel>
    {
        public Guid Id { get; set; }
    }
}