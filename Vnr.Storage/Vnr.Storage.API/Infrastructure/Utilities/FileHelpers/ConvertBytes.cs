using System;
using System.IO;

namespace Vnr.Storage.API.Infrastructure.Utilities.FileHelpers
{
    public static partial class FileHelpers
    {
        public static bool ByteArrayToFile(string fileName, byte[] data)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(data, 0, data.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }

        public static Stream ByteArrayToMemoryStream(byte[] data)
        {
            try
            {
                Stream stream = new MemoryStream(data);
                return stream;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Stream.Null;
            }
        }
    }
}