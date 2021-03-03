using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vnr.Storage.API.Infrastructure.Data.Entities;

namespace Vnr.Storage.API.Infrastructure.Data.Configuration
{
    public class RijndaelKeyEntityTypeConfiguration : IEntityTypeConfiguration<RijndaelKey>
    {
        public void Configure(EntityTypeBuilder<RijndaelKey> builder)
        {
            builder.HasData(new RijndaelKey
            {
                Id = 1,
                Key = "fjc0nLMh5acJhSz5XjN4X6zExZUShzYN11Twf6VSwFE=",
                IV = "QtgaRmKYbpvV4cPp3DtU/g=="
            });
        }
    }
}