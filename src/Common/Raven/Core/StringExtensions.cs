using Raven.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Raven
{
    public static class StringExtensions
    {
        /// <summary>
        /// 指示指定的字符串是 null 或者 System.String.Empty 字符串
        /// </summary>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 指示指定的字符串是 null、空或者仅由空白字符组成。
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 指示指定的字符串是 null、空或者仅由空白字符组成。
        /// </summary>
        public static bool IsMissing(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 将Json字符串转化为对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static object ToObject(this string value, JsonSerializerSettings? settings = null)
        {
            if (value != null)
            {
                return JsonConvert.DeserializeObject(value, settings);
            }

            return default;
        }

        /// <summary>
        /// 将Json字符串转化为指定类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string value, JsonSerializerSettings? settings = null)
        {
            if (value != null)
            {
                return JsonConvert.DeserializeObject<T>(value, settings);
            }

            return default;
        }

        /// <summary>
        /// 获取字符串的MD5 Hash值
        /// </summary>
        public static string ToMd5Hash(this string value)
        {
            return HashManager.GetMd5(value);
        }

        /// <summary>
        /// 给URL添加查询参数
        /// </summary>
        /// <param name="url">URL字符串</param>
        /// <param name="queries">要添加的参数，形如："id=1,cid=2"</param>
        /// <returns></returns>
        public static string AddUrlQuery(this string url, params string[] queries)
        {
            foreach (string query in queries)
            {
                if (!url.Contains("?"))
                {
                    url += "?";
                }
                else if (!url.EndsWith("&"))
                {
                    url += "&";
                }

                url = url + query;
            }
            return url;
        }

        /// <summary>
        /// 获取URL中指定参数的值，不存在返回空字符串
        /// </summary>
        public static string GetUrlQuery(this string url, string key)
        {
            Uri uri = new Uri(url);
            string query = uri.Query;
            if (query.IsNullOrEmpty())
            {
                return string.Empty;
            }
            query = query.TrimStart('?');
            var dict = (from m in query.Split("&")
                        let strs = m.Split("=")
                        select new KeyValuePair<string, string>(strs[0], strs[1]))
                .ToDictionary(m => m.Key, m => m.Value);
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            return string.Empty;
        }

        /// <summary>
        /// 给URL添加 # 参数
        /// </summary>
        /// <param name="url">URL字符串</param>
        /// <param name="query">要添加的参数</param>
        /// <returns></returns>
        public static string AddHashFragment(this string url, string query)
        {
            if (!url.Contains("#"))
            {
                url += "#";
            }

            return url + query;
        }

        /// <summary>
        /// 将字符串转换为<see cref="byte"/>[]数组，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        public static byte[] ToBytes(this string value, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding.GetBytes(value);
        }

        static string ToUnicodeString(this string source)
        {
            Regex regex = new Regex(@"[^\u0000-\u00ff]");
            return regex.Replace(source, m => string.Format(@"\u{0:x4}", (short)m.Value[0]));
        }

        /// <summary>
        /// 将形如“\u7f16\u7801”的Unicode字符串解码
        /// </summary>
        public static string FromUnicodeString(this string source)
        {
            Regex regex = new Regex(@"\\u([0-9a-fA-F]{4})", RegexOptions.Compiled);
            return regex.Replace(source,
                m =>
                {
                    short s;
                    if (short.TryParse(m.Groups[1].Value, NumberStyles.HexNumber, CultureInfo.InstalledUICulture, out s))
                    {
                        return "" + (char)s;
                    }
                    return m.Value;
                });
        }

        /// <summary>
        /// 将驼峰字符串按单词拆分并转换成小写，再以特定字符串分隔
        /// </summary>
        /// <param name="str">待转换的字符串</param>
        /// <param name="splitStr">分隔符字符</param>
        /// <returns></returns>
        public static string UpperToLowerAndSplit(this string str, string splitStr = "-")
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            List<string> words = new List<string>();
            while (str.Length > 0)
            {
                char c = str.FirstOrDefault(char.IsUpper);
                if (c == default(char))
                {
                    words.Add(str);
                    break;
                }
                int upperIndex = str.IndexOf(c);
                if (upperIndex < 0) //admin
                {
                    return str;
                }
                if (upperIndex > 0) //adminAdmin
                {
                    string first = str.Substring(0, upperIndex);
                    words.Add(first);
                    str = str.Substring(upperIndex, str.Length - upperIndex);
                    continue;
                }
                str = char.ToLower(str[0]) + str.Substring(1, str.Length - 1);
            }
            return words.ExpandAndToString(splitStr);
        }

        /// <summary>
        /// 将驼峰字符串的第一个字符小写
        /// </summary>
        public static string LowerFirstChar(this string str)
        {
            if (string.IsNullOrEmpty(str) || !char.IsUpper(str[0]))
            {
                return str;
            }
            if (str.Length == 1)
            {
                return char.ToLower(str[0]).ToString();
            }
            return char.ToLower(str[0]) + str.Substring(1, str.Length - 1);
        }

        /// <summary>
        /// 将小驼峰字符串的第一个字符大写
        /// </summary>
        public static string UpperFirstChar(this string str)
        {
            if (string.IsNullOrEmpty(str) || !char.IsLower(str[0]))
            {
                return str;
            }
            if (str.Length == 1)
            {
                return char.ToUpper(str[0]).ToString();
            }
            return char.ToUpper(str[0]) + str.Substring(1, str.Length - 1);
        }
    }
}
