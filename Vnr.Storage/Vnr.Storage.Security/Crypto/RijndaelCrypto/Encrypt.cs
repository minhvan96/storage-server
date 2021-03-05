using System.IO;
using System.Security.Cryptography;
using Vnr.Storage.Security.Utilities;

namespace Vnr.Storage.Security.Crypto.RijndaelCrypto
{
    public static partial class RijndaelCrypto
    {
        public static byte[] EncryptDataToBytes(byte[] Data, byte[] Key, byte[] IV)
        {
            RijndaelHelper.CanPerformEncrypt(Data, Key, IV);
            byte[] encryptedData;

            // Create an Rijndael object
            // with the specified key and IV.
            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
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

            // Return the encrypted bytes from the memory stream.
            return encryptedData;
        }

        public static byte[] DecryptDataFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            RijndaelHelper.CanPerformDecrypt(cipherText, Key, IV);

            byte[] decryptedData = null;

            // Create an Rijndael object
            // with the specified key and IV.
            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (BinaryReader binaryReader = new BinaryReader(csDecrypt))
                        {
                            //decryptedData = binaryReader.ReadBytes((int)csDecrypt.Length);
                            decryptedData = binaryReader.ReadAllBytes();
                        }
                    }
                }
            }

            return decryptedData;
        }
    }
}