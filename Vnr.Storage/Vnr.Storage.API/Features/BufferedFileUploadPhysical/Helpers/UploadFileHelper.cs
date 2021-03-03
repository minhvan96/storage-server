using System.IO;
using Vnr.Storage.API.Infrastructure.Contants;
using Vnr.Storage.API.Infrastructure.Enums;

namespace Vnr.Storage.API.Features.BufferedFileUploadPhysical.Helpers
{
    public static class UploadFileHelper
    {
        public static string UploadFileLocation(string contentRootPath, string fileName, Archive archive)
        {
            return archive switch
            {
                Archive.Contract => Path.Combine(contentRootPath, PathConstants.ContractArchivePath, fileName),
                Archive.Salary => Path.Combine(contentRootPath, PathConstants.SalaryArchivePath, fileName),
                _ => string.Empty,
            };
        }
    }
}