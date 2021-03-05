using System.IO;
using System.Security.Cryptography;

namespace Vnr.Storage.Security.Crypto.AesCrypto
{
    public static partial class AesCrypto
    {
        public static byte[] EncryptDataToBytes(byte[] Data, byte[] Key, byte[] IV)
        {
            AesHelper.CanPerformEncrypt(Data, Key, IV);
            byte[] encryptedData;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (BinaryWriter binaryWriter = new BinaryWriter(csEncrypt))
                        {
                            binaryWriter.Write(Data);
                        }
                        encryptedData = msEncrypt.ToArray();
                    }
                }
            }

            return encryptedData;
        }

        public static bool EncryptDataAndSaveToFile(byte[] Data, byte[] Key, byte[] IV, string absolutePath)
        {
            AesHelper.CanPerformEncrypt(Data, Key, IV);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (FileStream msEncrypt = File.Create(absolutePath))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (BinaryWriter binaryWriter = new BinaryWriter(csEncrypt))
                        {
                            binaryWriter.Write(Data);
                        }
                    }
                }
            }

            return true;
        }
    }
}