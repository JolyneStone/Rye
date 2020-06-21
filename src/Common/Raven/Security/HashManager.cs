using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Raven.Security
{
    /// <summary>
    /// Hash操作类
    /// </summary>
    public static class HashManager
    {
        /// <summary>
        /// 获取字符串的MD5哈希值，默认编码为UTF8
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="encoding">编码</param>
        public static string GetMd5(string value, Encoding encoding = null)
        {
            Check.NotNullOrEmpty(value, nameof(value));
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            byte[] bytes = encoding.GetBytes(value);
            return GetMd5(bytes);
        }

        /// <summary>
        /// 获取字节数组的MD5哈希值
        /// </summary>
        /// <param name="bytes">字节数组</param>
        public static string GetMd5(byte[] bytes)
        {
            Check.NotNullOrEmpty(bytes, nameof(bytes));
            StringBuilder sb = new StringBuilder();
            using (MD5 hash = new MD5CryptoServiceProvider())
            {
                bytes = hash.ComputeHash(bytes);
                foreach (byte b in bytes)
                {
                    sb.AppendFormat("{0:x2}", b);
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 获取字符串的SHA1哈希值，默认编码为UTF8
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="encoding">编码</param>
        public static string GetSha1(string value, Encoding encoding = null)
        {
            Check.NotNullOrEmpty(value, nameof(value));

            StringBuilder sb = new StringBuilder();
            using (SHA1Managed hash = new SHA1Managed())
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }
                byte[] bytes = hash.ComputeHash(encoding.GetBytes(value));
                foreach (byte b in bytes)
                {
                    sb.AppendFormat("{0:x2}", b);
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 获取字符串的Sha256哈希值，默认编码为UTF8
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="encoding">编码</param>
        public static string GetSha256(string value, Encoding encoding = null)
        {
            Check.NotNullOrEmpty(value, nameof(value));

            StringBuilder sb = new StringBuilder();
            using (SHA256Managed hash = new SHA256Managed())
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }
                byte[] bytes = hash.ComputeHash(encoding.GetBytes(value));
                foreach (byte b in bytes)
                {
                    sb.AppendFormat("{0:x2}", b);
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 获取字符串的Sha512哈希值，默认编码为UTF8
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="encoding">编码</param>
        public static string GetSha512(string value, Encoding encoding = null)
        {
            Check.NotNullOrEmpty(value, nameof(value));

            StringBuilder sb = new StringBuilder();
            using (SHA512Managed hash = new SHA512Managed())
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }
                byte[] bytes = hash.ComputeHash(encoding.GetBytes(value));
                foreach (byte b in bytes)
                {
                    sb.AppendFormat("{0:x2}", b);
                }
                return sb.ToString();
            }
        }
    }
}
