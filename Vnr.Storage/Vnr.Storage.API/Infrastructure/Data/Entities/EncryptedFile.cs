using System;
using System.ComponentModel.DataAnnotations;

namespace Vnr.Storage.API.Infrastructure.Data.Entities
{
    public class EncryptedFile
    {
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string FileName { get; set; }

        [MaxLength(512)]
        public string Path { get; set; }

        [MaxLength(512)]
        public string FullPath { get; set; }
    }
}