using System.ComponentModel.DataAnnotations;

namespace Vnr.Storage.API.Infrastructure.Data.Entities
{
    public class AesKey
    {
        public long Id { get; set; }

        [MaxLength(40)]
        public byte[] Key { get; set; }

        [MaxLength(40)]
        public byte[] IV { get; set; }
    }
}