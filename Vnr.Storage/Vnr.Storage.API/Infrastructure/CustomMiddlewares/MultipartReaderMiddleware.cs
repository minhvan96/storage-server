using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System.Linq;
using System.Threading.Tasks;
using Vnr.Storage.API.Infrastructure.Models;
using Vnr.Storage.API.Infrastructure.Utilities;
using Vnr.Storage.API.Infrastructure.Utilities.FileHelpers;

namespace Vnr.Storage.API.Infrastructure.CustomMiddlewares
{
    public class MultipartReaderMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        public MultipartReaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!MultipartRequestHelper.IsMultipartContentType(context.Request.ContentType))
            {
                await _next(context);
                return;
            }
            var boundary = MultipartRequestHelper.GetBoundary(
                            MediaTypeHeaderValue.Parse(context.Request.ContentType),
                            _defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, context.Request.Body);
            var section = await reader.ReadNextSectionAsync();

            while (section != null)
            {
                var errorModel = new FormFileErrorModel();
                var hasContentDispositionHeader =
                    ContentDispositionHeaderValue.TryParse(
                        section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (!MultipartRequestHelper
                        .HasFileContentDisposition(contentDisposition))
                    {
                        await _next(context);
                        return;
                    }
                    else
                    {
                        await FileHelpers.ProcessStreamedFile(
                            section, contentDisposition, errorModel, new string[]
                            { ".docx", ".xlsx", ".txt", ".pdf"}, 204857600, Enums.ValidateExtension.Encrypt);

                        if (errorModel.Errors.Any())
                        {
                            await _next(context);
                            return;
                        }
                    }
                }

                section = await reader.ReadNextSectionAsync();
            }
            await _next(context);
        }
    }
}