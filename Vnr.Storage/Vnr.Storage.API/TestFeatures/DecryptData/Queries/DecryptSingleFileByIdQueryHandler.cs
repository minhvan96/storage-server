using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Vnr.Storage.API.Infrastructure.Crypto.RijndaelCrypto;
using Vnr.Storage.API.Infrastructure.Data;
using Vnr.Storage.API.Infrastructure.Models;
using Vnr.Storage.API.Infrastructure.Utilities.FileHelpers;

namespace Vnr.Storage.API.Features.DecryptData.Queries
{
    public class DecryptSingleFileByIdQueryHandler : IRequestHandler<DecryptSingleFileByIdQuery, FileContentResultModel>
    {
        private readonly StorageContext _context;
        private readonly string _contentRootPath;

        public DecryptSingleFileByIdQueryHandler(IWebHostEnvironment env, StorageContext context)
        {
            _context = context;
            _contentRootPath = env.ContentRootPath;
        }

        public async Task<FileContentResultModel> Handle(DecryptSingleFileByIdQuery request, CancellationToken cancellationToken)
        {
            var encryptedFile = await _context.EncryptedFiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            var encryptedFileAbsolutePath = Path.Combine(_contentRootPath, encryptedFile.Path);
            var response = new FileContentResultModel();

            using (FileStream fs = File.Open(encryptedFileAbsolutePath, FileMode.Open))
            {
                byte[] data = new BinaryReader(fs).ReadBytes((int)fs.Length);

                var decryptedFileContent = await DecryptFileContent(data);

                response.StreamData = FileHelpers.ByteArrayToMemoryStream(decryptedFileContent);
                response.FileName = encryptedFile.FileName;

                return response;
            }

            throw new NotImplementedException();
        }

        private async Task<byte[]> DecryptFileContent(byte[] content)
        {
            RijndaelManaged myRijndael = new RijndaelManaged();

            var rijndaeData = await _context.RijndaelKeys.FirstOrDefaultAsync();
            myRijndael.Key = Convert.FromBase64String(rijndaeData.Key);
            myRijndael.IV = Convert.FromBase64String(rijndaeData.IV);

            var decryptedFileContent = RijndaelCrypto.DecryptDataFromBytes(content, myRijndael.Key, myRijndael.IV);
            return decryptedFileContent;
        }
    }
}