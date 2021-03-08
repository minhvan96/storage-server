using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using Vnr.Storage.API.Infrastructure.Models;

namespace Vnr.Storage.API.Features.DecryptData.Queries
{
    public class DecryptSingleFileByIdQuery : IRequest<FileContentResultModel>
    {
        [Required]
        public Guid Id { get; set; }
    }
}