using MediatR;
using System.ComponentModel.DataAnnotations;
using Vnr.Storage.API.Infrastructure.Models;

namespace Vnr.Storage.API.Features.Decrypt.Queries
{
    public class GetByFileNameQuery : IRequest<FileContentResultModel>
    {
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }

        [Required]
        [MaxLength(256)]
        public string FileName { get; set; }
    }
}