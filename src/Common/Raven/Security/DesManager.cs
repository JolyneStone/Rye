using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Raven.Security
{
    /// <summary>
    /// DES加密解密操作类
    /// </summary>
    public static class DesManager
    {
        //默认密钥向量 
        private static byte[] Keys = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        /// <summary> 
        /// DES加密字符串，默认编码为UTF8
        /// </summary> 
        /// <param name="value">待加密的字符串</param> 
        /// <param name="key">加密密钥,要求为16位</param> 
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns> 
        public static string DESEncrypt(string value, string key, Encoding encoding = null)
        {
            Check.NotNullOrEmpty(value, nameof(value));
            Check.NotNullOrEmpty(key, nameof(key));
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] rgbKey = encoding.GetBytes(key.Substring(0, 16));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = encoding.GetBytes(value);
            var DCSP = Aes.Create();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }
        /// <summary> 
        /// DES解密字符串，默认编码为UTF8
        /// </summary> 
        /// <param name="value">待解密的字符串</param> 
        /// <param name="key">解密密钥,要求为16位,和加密密钥相同</param> 
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns> 
        public static string DESDecrypt(string value, string key, Encoding encoding = null)
        {
            Check.NotNullOrEmpty(value, nameof(value));
            Check.NotNullOrEmpty(key, nameof(key));
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] rgbKey = encoding.GetBytes(key.Substring(0, 16));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Convert.FromBase64String(value);
            var DCSP = Aes.Create();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            Byte[] inputByteArrays = new byte[inputByteArray.Length];
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return encoding.GetString(mStream.ToArray());
        }
    }
}
