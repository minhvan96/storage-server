//namespace Vnr.Storage.Security.SecurityHelper.Utilities
//{
//    public static class SecurityUtility
//    {
//        static SecurityUtility()
//        {
//            EncryptionNormal = new EncryptionBase()
//            {
//                AlgorithmType = typeof(AesCryptoServiceProvider),
//                EncryptionKeyType = DefineResource.EncryptionType.KeyNormal
//            };

//            EncryptionAdvanced = new EncryptionBase()
//            {
//                AlgorithmType = typeof(RijndaelManaged),
//                EncryptionKeyType = DefineResource.EncryptionType.KeyAdvance
//            };
//        }

//        /// <summary>
//        /// Use Encryption Mode Normal
//        /// </summary>
//        public readonly static EncryptionBase EncryptionNormal;

//        /// <summary>
//        /// Use Encryption Mode Advanced
//        /// </summary>
//        public readonly static EncryptionBase EncryptionAdvanced;

//        /// <summary>
//        /// Config Use Encryption New Method
//        /// </summary>
//        public static bool UseEncryptionNew = false;
//    }
//}