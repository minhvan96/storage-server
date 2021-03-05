using System.IO;
using System.Security.Cryptography;

namespace Vnr.Storage.Security.Crypto.RijndaelCrypto
{
    public static partial class RijndaelCrypto
    {
        public static byte[] EncryptDataToBytes(byte[] Data, byte[] Key, byte[] IV)
        {
            RijndaelHelper.CanPerformEncrypt(Data, Key, IV);
            byte[] encryptedData;

            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

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
            RijndaelHelper.CanPerformEncrypt(Data, Key, IV);

            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

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