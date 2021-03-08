using System.IO;
using Vnr.Storage.API.Configuration.Contants;
using Vnr.Storage.API.Infrastructure.Enums;

namespace Vnr.Storage.API.Features.UploadPhysical.Helpers
{
    public static class UploadFileHelper
    {
        public static string GetFileNameWithEncryptExtension(string fileName, EncryptAlg encryptAlg)
        {
            return encryptAlg switch
            {
                EncryptAlg.None => fileName,
                EncryptAlg.AES => fileName + FileConstants.AesExtension,
                EncryptAlg.Rijndael => fileName + FileConstants.RijndaelExtension,
                _ => fileName
            };
        }

        public static string GetUploadAbsolutePath(string contentRootPath, string fileName, Archive archive)
        {
            return archive switch
            {
                Archive.Contract => Path.Combine(contentRootPath, PathConstants.ContractArchivePath, fileName),
                Archive.Salary => Path.Combine(contentRootPath, PathConstants.SalaryArchivePath, fileName),
                _ => string.Empty,
            };
        }

        public static string GetUploadRelativePath(string fileName, Archive archive)
        {
            return archive switch
            {
                Archive.Contract => Path.Combine(PathConstants.ContractArchivePath, fileName),
                Archive.Salary => Path.Combine(PathConstants.SalaryArchivePath, fileName),
                _ => string.Empty,
            };
        }
    }
}