using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Vnr.Storage.API.Infrastructure.Data;
using Vnr.Storage.API.Infrastructure.Models;
using Vnr.Storage.Security.Crypto.Symmetric;

namespace Vnr.Storage.API.Features.Decrypt.Queries
{
    public class GetByFileNameQueryHandler : IRequestHandler<GetByFileNameQuery, FileContentResultModel>
    {
        private readonly StorageContext _context;
        private readonly string _contentRootPath;

        public GetByFileNameQueryHandler(IWebHostEnvironment env, StorageContext context)
        {
            _context = context;
            _contentRootPath = env.ContentRootPath;
        }

        public async Task<FileContentResultModel> Handle(GetByFileNameQuery request, CancellationToken cancellationToken)
        {
            var encryptedFileAbsolutePath = Path.Combine(_contentRootPath, "Archive", request.CategoryName, request.FileName);
            var response = new FileContentResultModel();

            using (FileStream fs = File.Open(encryptedFileAbsolutePath, FileMode.Open))
            {
                byte[] data = new BinaryReader(fs).ReadBytes((int)fs.Length);

                response.StreamData = await DecryptFileContent(data);
                response.FileName = Path.GetFileNameWithoutExtension(request.FileName);

                return response;
            }
        }

        private async Task<Stream> DecryptFileContent(byte[] content)
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