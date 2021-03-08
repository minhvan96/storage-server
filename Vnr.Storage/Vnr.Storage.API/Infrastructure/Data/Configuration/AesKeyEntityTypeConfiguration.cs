using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography;
using Vnr.Storage.API.Infrastructure.Data.Entities;

namespace Vnr.Storage.API.Infrastructure.Data.Configuration
{
    public class AesKeyEntityTypeConfiguration : IEntityTypeConfiguration<AesKey>
    {
        public void Configure(EntityTypeBuilder<AesKey> builder)
        {
            var aes = Aes.Create();
            builder.HasData(new AesKey
            {
                Id = 1,
                Key = aes.Key,
                IV = aes.IV
            });
        }
    }
}