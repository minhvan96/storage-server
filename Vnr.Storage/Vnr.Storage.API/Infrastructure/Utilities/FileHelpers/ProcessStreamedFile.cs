using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Vnr.Storage.API.Infrastructure.Enums;
using Vnr.Storage.API.Infrastructure.Models;

namespace Vnr.Storage.API.Infrastructure.Utilities.FileHelpers
{
    public static partial class FileHelpers
    {
        public static async Task<byte[]> ProcessStreamedFile(
            MultipartSection section, ContentDispositionHeaderValue contentDisposition,
            FormFileErrorModel errorModel, string[] permittedExtensions, long sizeLimit, ValidateExtension purpose)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await section.Body.CopyToAsync(memoryStream);

                    // Check if the file is empty or exceeds the size limit.
                    if (memoryStream.Length == 0)
                    {
                        errorModel.Errors.Add("File", "The file is empty.");
                    }
                    else if (memoryStream.Length > sizeLimit)
                    {
                        var megabyteSizeLimit = sizeLimit / 1048576;
                        errorModel.Errors.Add("File", $"The file exceeds {megabyteSizeLimit:N1} MB.");
                    }
                    else if (purpose == ValidateExtension.Encrypt && !IsValidFileExtensionAndSignature(
                        contentDisposition.FileName.Value, memoryStream,
                        permittedExtensions))
                    {
                        errorModel.Errors.Add("File", "The file type isn't permitted or the file's " +
                            "signature doesn't match the file's extension.");
                    }
                    else if (purpose == ValidateExtension.Decrypt && !IsValidFileExtensionForDecrypt(
                        contentDisposition.FileName.Value, memoryStream,
                        permittedExtensions))
                    {
                        errorModel.Errors.Add("File", "The file type isn't permitted or the file's " +
                            "signature doesn't match the file's extension.");
                    }
                    else
                    {
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                errorModel.Errors.Add("File",
                    "The upload failed. Please contact the Help Desk " +
                    $" for support. Error: {ex.HResult}");
                // Log the exception
            }

            return new byte[0];
        }

        private static bool IsValidFileExtensionForDecrypt(string fileName, Stream data, string[] permittedDecryptExtension)
        {
            if (string.IsNullOrEmpty(fileName) || data == null || data.Length == 0)
            {
                return false;
            }
            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedDecryptExtension.Contains(ext))
            {
                return false;
            }
            return true;
        }
    }
}