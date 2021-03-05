using System;

namespace Vnr.Storage.Security.Crypto.AesCrypto
{
    public static class AesHelper
    {
        public static void CanPerformEncrypt(byte[] data, byte[] key, byte[] iv)
        {
            if (data == null || data.Length <= 0)
                throw new ArgumentNullException("data");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");
        }

        public static void CanPerformDecrypt(byte[] data, byte[] key, byte[] iv)
        {
            if (data == null || data.Length <= 0)
                throw new ArgumentNullException("data");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");
        }
    }
}