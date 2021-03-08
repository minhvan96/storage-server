using System.IO;
using System.Threading.Tasks;

namespace Vnr.Storage.API.Infrastructure.Utilities.FileHelpers
{
    public static partial class FileHelpers
    {
        public static async Task<Stream> ReadFileToMemoryStream(string absolutePath)
        {
            var memoryStream = new MemoryStream();
            using (var fileStream = new FileStream(absolutePath, FileMode.Open, FileAccess.Read))
            {
                await memoryStream.CopyToAsync(fileStream);
            }
            return memoryStream;
        }
    }
}