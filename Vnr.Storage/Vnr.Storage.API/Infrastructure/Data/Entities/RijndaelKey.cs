using System.ComponentModel.DataAnnotations;

namespace Vnr.Storage.API.Infrastructure.Data.Entities
{
    public class RijndaelKey
    {
        public long Id { get; set; }

        [MaxLength(256)]
        public string Key { get; set; }

        [MaxLength(256)]
        public string IV { get; set; }
    }
}