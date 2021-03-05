using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;

namespace Vnr.Storage.Security.SecurityHelper.Utilities.EncryptionBase
{
    public partial class EncryptionBase
    {
        #region [Static Variables]

        private readonly static Encoding __defaultEncoding = Encoding.UTF8;

        /// <summary>
        /// Cache Crypt Text
        /// </summary>
        private static readonly ObjectCache __cryptorStorage = MemoryCache.Default;

        private static readonly TimeSpan __cryptorStorageExpire = TimeSpan.FromMinutes(60 * 24);

        /// <summary>
        /// Cache Encrypt Transform
        /// </summary>
        private static readonly SortedDictionary<string, ICryptoTransform> __encryptTransformCache = new SortedDictionary<string, ICryptoTransform>();

        private static readonly object __encryptTransformCacheLock = new object();

        /// <summary>
        /// Cache Decrypt Transform
        /// </summary>
        private static readonly SortedDictionary<string, ICryptoTransform> __decryptTransformCache = new SortedDictionary<string, ICryptoTransform>();

        private static readonly object __decryptTransformCacheLock = new object();

        private static readonly SortedDictionary<string, (byte[], byte[])> __deriveBytesCache = new SortedDictionary<string, (byte[], byte[])>();
        private static readonly object __deriveBytesCacheLock = new object();

        private readonly string instanceName;

        #endregion [Static Variables]

        /// <summary>
        /// Base Type SymmetricAlgorithm
        /// </summary>
        public Type AlgorithmType { set; get; } = typeof(AesCryptoServiceProvider);

        /// <summary>
        /// Base Type SymmetricAlgorithm
        /// </summary>
        //public DefineResource.EncryptionType EncryptionKeyType { set; get; } = DefineResource.EncryptionType.KeyNormal;

        public EncryptionBase()
        {
            this.instanceName = this.GetType().Name;
        }
    }
}