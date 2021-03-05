using System.IO;
using System.Security.Cryptography;
using Vnr.Storage.Security.Utilities;

namespace Vnr.Storage.Security.Crypto.RijndaelCrypto
{
    public static partial class RijndaelCrypto
    {
        public static byte[] DecryptDataFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            RijndaelHelper.CanPerformDecrypt(cipherText, Key, IV);

            byte[] decryptedData = null;

            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

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
            RijndaelHelper.CanPerformDecrypt(Data, Key, IV);

            byte[] decryptedData = null;

            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

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