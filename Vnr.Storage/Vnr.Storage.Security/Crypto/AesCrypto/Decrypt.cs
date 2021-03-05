using System.IO;
using System.Security.Cryptography;
using Vnr.Storage.Security.Utilities;

namespace Vnr.Storage.Security.Crypto.AesCrypto
{
    public static partial class AesCrypto
    {
        public static byte[] DecryptDataFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            AesHelper.CanPerformDecrypt(cipherText, Key, IV);

            byte[] decryptedData = null;

            using (Rijndael aesAlg = Rijndael.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (BinaryReader binaryReader = new BinaryReader(csDecrypt))
                        {
                            decryptedData = binaryReader.ReadAllBytes();
                        }
                    }
                }
            }

            return decryptedData;
        }

        public static Stream DecryptDataToStream(byte[] Data, byte[] Key, byte[] IV)
        {
            AesHelper.CanPerformDecrypt(Data, Key, IV);

            byte[] decryptedData = null;

            using (Rijndael aesAlg = Rijndael.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Data))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (BinaryReader binaryReader = new BinaryReader(csDecrypt))
                        {
                            decryptedData = binaryReader.ReadAllBytes();
                        }
                    }
                }
            }
            return new MemoryStream(decryptedData);
        }
    }
}