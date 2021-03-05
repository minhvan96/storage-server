using System.Text;

namespace Vnr.Storage.Security.SecurityHelper.Utilities
{
    public static class SecurityUtilityExtentions
    {
        private readonly static string __hexAlphabet = "0123456789ABCDEF";

        /// <summary>
        /// Convert Bytes To Hex String
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHex(this byte[] bytes)
        {
            var result = new StringBuilder(bytes.Length * 2);

            foreach (byte B in bytes)
            {
                result.Append(__hexAlphabet[(B >> 4)]);
                result.Append(__hexAlphabet[(B & 0xF)]);
            }

            return result.ToString();
        }

        private readonly static int[] __hexValue = new int[]
        {
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05,
            0x06, 0x07, 0x08, 0x09, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x0A,
            0x0B, 0x0C, 0x0D, 0x0E, 0x0F
        };

        /// <summary>
        /// Convert Hex String To Bytes
        /// </summary>
        /// <param name="hexText"></param>
        /// <returns></returns>
        public static byte[] FromHex(this string hexText)
        {
            hexText = hexText.ToUpper();
            byte[] bytes = new byte[hexText.Length / 2];

            for (int x = 0, i = 0; i < hexText.Length; i += 2, x += 1)
            {
                bytes[x] = (byte)(__hexValue[hexText[i + 0] - '0'] << 4 |
                                  __hexValue[hexText[i + 1] - '0']);
            }

            return bytes;
        }
    }
}