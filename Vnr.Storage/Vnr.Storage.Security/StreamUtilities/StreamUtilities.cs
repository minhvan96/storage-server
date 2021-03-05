using System.IO;

namespace Vnr.Storage.Security.Utilities
{
    public static class StreamUtilities
    {
        private const int BUFFER_SIZE = 4096;

        public static byte[] ReadAllBytes(this BinaryReader reader)
        {
            using (var ms = new MemoryStream())
            {
                byte[] buffer = new byte[BUFFER_SIZE];
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);
                return ms.ToArray();
            }
        }
    }
}