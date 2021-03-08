using System.IO;
using System.Threading.Tasks;

namespace Vnr.Storage.API.Infrastructure.Utilities.FileHelpers
{
    public static partial class FileHelpers
    {
        public static async Task<bool> UploadFile(byte[] streamedFileContent, string absolutePath)
        {
            using (var fileStream = File.Create(absolutePath))
            {
                await fileStream.WriteAsync(streamedFileContent);
                return true;
            }
        }
    }
}