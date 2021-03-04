using Microsoft.AspNetCore.StaticFiles;

namespace Vnr.Storage.API.Infrastructure.Utilities.FileHelpers
{
    public static partial class FileHelpers
    {
        public static string GetMIMEType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }
}