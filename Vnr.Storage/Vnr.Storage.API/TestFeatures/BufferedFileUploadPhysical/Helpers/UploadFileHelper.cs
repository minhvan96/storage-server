using System.IO;
using Vnr.Storage.API.Configuration.Contants;

namespace Vnr.Storage.API.Features.BufferedFileUploadPhysical.Helpers
{
    public static class UploadFileHelper
    {
        public static string GetUploadAbsolutePath(string contentRootPath, string fileName, Infrastructure.Enums.Archive archive)
        {
            return archive switch
            {
                Infrastructure.Enums.Archive.Contract => Path.Combine(contentRootPath, PathConstants.ContractArchivePath, fileName),
                Infrastructure.Enums.Archive.Salary => Path.Combine(contentRootPath, PathConstants.SalaryArchivePath, fileName),
                _ => string.Empty,
            };
        }

        public static string GetUploadRelativePath(string fileName, Infrastructure.Enums.Archive archive)
        {
            return archive switch
            {
                Infrastructure.Enums.Archive.Contract => Path.Combine(PathConstants.ContractArchivePath, fileName),
                Infrastructure.Enums.Archive.Salary => Path.Combine(PathConstants.SalaryArchivePath, fileName),
                _ => string.Empty,
            };
        }
    }
}