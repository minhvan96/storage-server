using System.IO;

namespace Vnr.Storage.API.Infrastructure.Models
{
    public class FileContentResultModel
    {
        public Stream StreamData { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}