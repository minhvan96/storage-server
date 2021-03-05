using System.IO;
using System.Security.Cryptography;
using Vnr.Storage.Security.Utilities;

namespace Vnr.Storage.Security.Crypto.Symmetric
{
    public static partial class SymmetricCrypto
    {
        public static byte[] DecryptDataFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            SymmetricCryptoHelper.CanPerformDecrypt(cipherText, Key, IV);

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

        public static Stream DecryptDataToStream(byte[] Data, byte[] Key, byte[] IV, CryptoAlgorithm crypAlg = CryptoAlgorithm.Rijndael)
        {
            SymmetricCryptoHelper.CanPerformDecrypt(Data, Key, IV);

            byte[] decryptedData = null;

            if (crypAlg == CryptoAlgorithm.Rijndael)
            {
                using (Rijndael rijAlg = Rijndael.Create())
                {
                    rijAlg.Key = Key;
                    rijAlg.IV = IV;

                    ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                    decryptedData = GetDecryptedDataFromCryptoStream(Data, decryptor);
                }
                return new MemoryStream(decryptedData);
            }
            else
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    decryptedData = GetDecryptedDataFromCryptoStream(Data, decryptor);
                }
                return new MemoryStream(decryptedData);
            }
        }

        private static byte[] GetDecryptedDataFromCryptoStream(byte[] Data, ICryptoTransform decryptor)
        {
            using (MemoryStream msDecrypt = new MemoryStream(Data))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (BinaryReader binaryReader = new BinaryReader(csDecrypt))
                    {
                        return binaryReader.ReadAllBytes();
                    }
                }
            }
        }
    }
}