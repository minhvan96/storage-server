//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Security.Cryptography;
//using System.Text;

//namespace Vnr.Storage.Security.SecurityHelper.Utilities.EncryptionBase
//{
//    public partial class EncryptionBase
//    {
//        protected Stream ExecuteEncrypt(Stream originStream, Stream outCryptStream, long? readFromSeek, bool tryExecute = false)
//        {
//            try
//            {
//                var key = GetCryptKey();

//                lock (__deriveBytesCacheLock)
//                {
//                    if (!__deriveBytesCache.ContainsKey(key))
//                    {
//                        var deriveBytes = new Rfc2898DeriveBytes(key, DefineResource.Encryption.ExtendKey);
//                        __deriveBytesCache.Add(key, (deriveBytes.GetBytes(16), deriveBytes.GetBytes(32)));
//                    }
//                }

//                var saltPassword = __deriveBytesCache[key];
//                var encryptTransformCacheKey = $"{key}|||{this.AlgorithmType.Name}";

//                lock (__encryptTransformCacheLock)
//                {
//                    if (!__encryptTransformCache.ContainsKey(encryptTransformCacheKey) || tryExecute)
//                    {
//                        var symmetricAlgorithm = (SymmetricAlgorithm)Activator.CreateInstance(this.AlgorithmType);
//                        symmetricAlgorithm.IV = saltPassword.Item1;
//                        symmetricAlgorithm.Key = saltPassword.Item2;
//                        __encryptTransformCache[encryptTransformCacheKey] = symmetricAlgorithm.CreateEncryptor();
//                    }
//                }

//                var encryptTransform = __encryptTransformCache[encryptTransformCacheKey];
//                var encryptStream = new CryptoStream(outCryptStream, encryptTransform, CryptoStreamMode.Write);
//                originStream.Seek(readFromSeek ?? 0, SeekOrigin.Begin);
//                originStream.CopyTo(encryptStream);
//                encryptStream.FlushFinalBlock();

//                return encryptStream;
//            }
//            catch (Exception ex)
//            {
//                if (!tryExecute && ex.Message.IndexOf("Padding is invalid and cannot be removed") >= 0)
//                    return ExecuteEncrypt(originStream, outCryptStream, readFromSeek, true);
//                throw ex;
//            }
//        }
//    }
//}