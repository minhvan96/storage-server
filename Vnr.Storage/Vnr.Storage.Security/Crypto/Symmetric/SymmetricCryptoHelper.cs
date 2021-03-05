using System;

namespace Vnr.Storage.Security.Crypto.Symmetric
{
    public static class SymmetricCryptoHelper
    {
        public static void CanPerformEncrypt(byte[] data, byte[] key, byte[] iv)
        {
            if (data == null || data.Length <= 0)
                throw new ArgumentNullException(nameof(data));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));
        }

        public static void CanPerformDecrypt(byte[] data, byte[] key, byte[] iv)
        {
            if (data == null || data.Length <= 0)
                throw new ArgumentNullException(nameof(data));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));
        }
    }

    public enum CryptoAlgorithm
    {
        Rijndael,
        Aes
    }
}