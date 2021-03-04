using System;

namespace Vnr.Storage.API.Infrastructure.Crypto
{
    public static class RijndaelHelper
    {
        public static void CanPerformEncrypt(byte[] data, byte[] key, byte[] iv)
        {
            if (data == null || data.Length <= 0)
                throw new ArgumentNullException("Empty Data");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("IV");
        }

        public static void CanPerformDecrypt(byte[] data, byte[] key, byte[] iv)
        {
            if (data == null || data.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("IV");
        }
    }
}