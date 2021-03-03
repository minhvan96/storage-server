using Microsoft.AspNetCore.Mvc;

namespace Vnr.Api.Platform.Common.Command
{
    public abstract class CreateCommand<TRequest>
    {
        [FromBody]
        public TRequest Request { get; set; }
    }
}