using Microsoft.AspNetCore.Mvc;

namespace Vnr.Storage.API.Infrastructure.Common.Command
{
    public abstract class CreateCommand<TRequest>
    {
        [FromBody]
        public TRequest Request { get; set; }
    }
}