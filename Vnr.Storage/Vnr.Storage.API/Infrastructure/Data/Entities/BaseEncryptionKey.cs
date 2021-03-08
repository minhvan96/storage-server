using System.ComponentModel.DataAnnotations;

namespace Vnr.Storage.API.Infrastructure.Data.Entities
{
    public class BaseEncryptionKey
    {
        [MaxLength(256)]
        public string Key { get; set; }

        [MaxLength(256)]
        public string IV { get; set; }
    }
}