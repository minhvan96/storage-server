﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Vnr.Storage.API.Features.DecryptData.Commands;
using Vnr.Storage.API.Infrastructure;

namespace Vnr.Storage.API.Features.DecryptData
{
    [Consumes("multipart/form-data")]
    [Route("api/[controller]")]
    public class DecryptController : ApiControllerBase
    {
        public DecryptController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public Task<IActionResult> DecryptFile(DecryptFileCommand command)
            => HandleRequest(command);
    }
}