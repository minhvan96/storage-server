using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Vnr.Storage.API.Configuration.Contants;
using Vnr.Storage.API.Infrastructure.BaseResponse;
using Vnr.Storage.API.Infrastructure.Data;
using Vnr.Storage.API.Infrastructure.Models;
using Vnr.Storage.API.Infrastructure.Utilities.FileHelpers;
using Vnr.Storage.Security.Crypto.Symmetric;

namespace Vnr.Storage.API.Features.Download.Queries
{
    public class GetByFileNameQueryHandler : IRequestHandler<GetByFileNameQuery, ResponseModel<FileContentResultModel>>
    {
        private readonly StorageContext _context;
        private readonly string _contentRootPath;

        public GetByFileNameQueryHandler(IWebHostEnvironment env, StorageContext context)
        {
            _context = context;
            _contentRootPath = env.ContentRootPath;
        }

        public async Task<ResponseModel<FileContentResultModel>> Handle(GetByFileNameQuery request, CancellationToken cancellationToken)
        {
            var fileExtension = Path.GetExtension(request.FileName);
            var categoryPath = Path.Combine(_contentRootPath, "Archive", request.CategoryName);
            var fileAbsolutePath = Path.Combine(categoryPath, request.FileName);
            var fileContentResult = new FileContentResultModel();
            if (fileExtension != FileConstants.AesExtension && fileExtension != FileConstants.RijndaelExtension)
            {
                if (!Directory.Exists(categoryPath))
                    return ResponseProvider.NotFound<FileContentResultModel>(nameof(request.CategoryName));
                if (!File.Exists(fileAbsolutePath))
                    return ResponseProvider.NotFound<FileContentResultModel>(nameof(request.FileName));

                fileContentResult.StreamData = await FileHelpers.ReadFileToMemoryStream(fileAbsolutePath);
                fileContentResult.FileName = request.FileName;
            }

            using (FileStream fs = File.Open(fileAbsolutePath, FileMode.Open))
            {
                byte[] data = new BinaryReader(fs).ReadBytes((int)fs.Length);

                fileContentResult.StreamData = await DecryptFile(data, fileExtension);
                fileContentResult.FileName = Path.GetFileNameWithoutExtension(request.FileName);

                return ResponseProvider.Ok(fileContentResult);
            }
        }

        public async Task<Stream> DecryptFile(byte[] content, string fileExtension)
        {
            if (fileExtension == FileConstants.AesExtension)
            {
                var aesKey = await _context.AesKeys.FirstOrDefaultAsync();

                var decryptedFileContent = SymmetricCrypto.DecryptDataToStream(content, aesKey.Key, aesKey.IV, CryptoAlgorithm.Aes);
                return decryptedFileContent;
            }
            else
            {
                RijndaelManaged myRijndael = new RijndaelManaged();

                var rijndaeData = await _context.RijndaelKeys.FirstOrDefaultAsync();
                myRijndael.Key = Convert.FromBase64String(rijndaeData.Key);
                myRijndael.IV = Convert.FromBase64String(rijndaeData.IV);

                var decryptedFileContent = SymmetricCrypto.DecryptDataToStream(content, myRijndael.Key, myRijndael.IV);
                return decryptedFileContent;
            }
        }
    }
}