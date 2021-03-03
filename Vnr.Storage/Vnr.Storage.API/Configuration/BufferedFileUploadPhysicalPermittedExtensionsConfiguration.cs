namespace Vnr.Storage.API.Configuration
{
    public class BufferedFileUploadPhysicalPermittedExtensionsConfiguration
    {
        public string[] SingleFileUploadPermittedExtensions { get; set; }
        public string[] MultipleFileUploadPermittedExtensions { get; set; }
    }
}