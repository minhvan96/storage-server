using MediatR;
using System.ComponentModel.DataAnnotations;
using Vnr.Storage.API.Infrastructure.BaseResponse;
using Vnr.Storage.API.Infrastructure.Models;

namespace Vnr.Storage.API.Features.Download.Queries
{
    public class GetByFileNameQuery : IRequest<ResponseModel<FileContentResultModel>>
    {
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }

        [Required]
        [MaxLength(256)]
        public string FileName { get; set; }
    }
}