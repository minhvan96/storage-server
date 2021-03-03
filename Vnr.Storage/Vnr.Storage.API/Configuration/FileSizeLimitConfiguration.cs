namespace Vnr.Storage.API.Configuration
{
    public class FileSizeLimitConfiguration
    {
        public long StreamFileSizeLimit { get; set; }
        public long DefaultFileSizeLimit { get; set; }
        public long DocxFileSizeLimit { get; set; }
        public long ExcelFileSizeLimit { get; set; }
        public long PdfFileSizeLimit { get; set; }
    }
}