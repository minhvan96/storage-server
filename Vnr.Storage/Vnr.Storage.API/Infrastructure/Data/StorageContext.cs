using Microsoft.EntityFrameworkCore;
using Vnr.Storage.API.Infrastructure.Data.Entities;

namespace Vnr.Storage.API.Infrastructure.Data
{
    public class StorageContext : DbContext
    {
        public StorageContext(DbContextOptions<StorageContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StorageContext).Assembly);
        }

        public DbSet<RijndaelKey> RijndaelKeys { get; set; }
        public DbSet<FilePath> FilePaths { get; set; }
    }
}