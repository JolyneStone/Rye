using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Monica.Security
{
    /// <summary>
    /// Hmac操作类
    /// </summary>
    public static class HmacManager
    {
        /// <summary>
        /// 获取字符串的HMASHA1哈希值，默认编码为UTF8
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="encoding">编码</param>
        public static string GetHmaSha1(string value, Encoding encoding = null)
        {
            Check.NotNullOrEmpty(value, nameof(value));

            StringBuilder sb = new StringBuilder();
            using (HMACSHA1 hash = new HMACSHA1())
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }
                byte[] bytes = hash.ComputeHash(encoding.GetBytes(value));
                return Convert.ToBase64String(bytes);
            }
        }

        /// <summary>
        /// 获取字符串的HMASha256哈希值，默认编码为UTF8
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="encoding">编码</param>
        public static string GetHmaSha256(string value, Encoding encoding = null)
        {
            Check.NotNullOrEmpty(value, nameof(value));

            StringBuilder sb = new StringBuilder();
            using (HMACSHA256 hash = new HMACSHA256())
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }
                byte[] bytes = hash.ComputeHash(encoding.GetBytes(value));
                return Convert.ToBase64String(bytes);
            }
        }

        /// <summary>
        /// 获取字符串的HMASha512哈希值，默认编码为UTF8
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="encoding">编码</param>
        public static string GetHmaSha512(string value, Encoding encoding = null)
        {
            Check.NotNullOrEmpty(value, nameof(value));

            StringBuilder sb = new StringBuilder();
            using (HMACSHA512 hash = new HMACSHA512())
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }
                byte[] bytes = hash.ComputeHash(encoding.GetBytes(value));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
