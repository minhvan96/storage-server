namespace Vnr.Storage.API.Infrastructure.Data.Entities
{
    public class RijndaelKey
    {
        public long Id { get; set; }

        public string Key { get; set; }
        public string IV { get; set; }
    }
}