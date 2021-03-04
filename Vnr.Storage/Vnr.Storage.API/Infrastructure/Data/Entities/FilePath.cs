using System;

namespace Vnr.Storage.API.Infrastructure.Data.Entities
{
    public class EncryptedFile
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FullPath { get; set; }
    }
}