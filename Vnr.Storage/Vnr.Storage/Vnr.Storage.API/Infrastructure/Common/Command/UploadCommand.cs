using Microsoft.AspNetCore.Mvc;

namespace Vnr.Storage.API.Infrastructure.Common.Command
{
    public class UploadCommand<TRequest>
    {
        [FromBody]
        public TRequest Request { get; set; }
    }
}