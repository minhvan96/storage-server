using Microsoft.AspNetCore.Mvc;

namespace Vnr.Api.Platform.Common.Command
{
    public class UploadCommand<TRequest>
    {
        [FromBody]
        public TRequest Request { get; set; }
    }
}