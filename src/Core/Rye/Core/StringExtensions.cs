using Rye;
using Rye.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Web;

namespace Rye
{
    public static class StringExtensions
    {
        // 使用正则表达式一次性过滤所有不可见字符，包括换行符
        private static readonly Regex InvisibleCharRegex = new Regex(
            @"[\u0000-\u001F\u007F-\u009F\u200B-\u200D\u2028-\u2029\uFEFF\r\n]",
            RegexOptions.Compiled
        );

        /// <summary>
        /// 清理输入字符串，移除所有不可见字符
        /// </summary>
        /// <param name="input">原始输入</param>
        /// <returns>清理后的字符串</returns>
        public static string CleanStr(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // 使用正则表达式一次性移除所有不可见字符
            return InvisibleCharRegex.Replace(input, "").Trim();
        }

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

        /// <summary>
        /// 将byte[]数组转换为Base64字符串
        /// </summary>
        public static string ToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 将字符串转换为Base64字符串，默认编码为Encoding.UTF8
        /// </summary>
        /// <param name="source">正常的字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns>Base64字符串</returns>
        public static string ToBase64String(this string source, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return Convert.ToBase64String(encoding.GetBytes(source));
        }

        /// <summary>
        /// 将Base64字符串转换为正常字符串，默认编码为Encoding.UTF8
        /// </summary>
        /// <param name="base64String">Base64字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns>正常字符串</returns>
        public static string FromBase64String(this string base64String, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] bytes = Convert.FromBase64String(base64String);
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 将字符串进行UrlDecode解码
        /// </summary>
        /// <param name="source">待UrlDecode解码的字符串</param>
        /// <returns>UrlDecode解码后的字符串</returns>
        public static string ToUrlDecode(this string source)
        {
            return HttpUtility.UrlDecode(source);
        }

        /// <summary>
        /// 将字符串进行UrlEncode编码
        /// </summary>
        /// <param name="source">待UrlEncode编码的字符串</param>
        /// <returns>UrlEncode编码后的字符串</returns>
        public static string ToUrlEncode(this string source)
        {
            return HttpUtility.UrlEncode(source);
        }

        /// <summary>
        /// 将字符串进行HtmlDecode解码
        /// </summary>
        /// <param name="source">待HtmlDecode解码的字符串</param>
        /// <returns>HtmlDecode解码后的字符串</returns>
        public static string ToHtmlDecode(this string source)
        {
            return HttpUtility.HtmlDecode(source);
        }

        /// <summary>
        /// 将字符串进行HtmlEncode编码
        /// </summary>
        /// <param name="source">待HtmlEncode编码的字符串</param>
        /// <returns>HtmlEncode编码后的字符串</returns>
        public static string ToHtmlEncode(this string source)
        {
            return HttpUtility.HtmlEncode(source);
        }

        /// <summary>
        /// 将字符串转换为十六进制字符串，默认编码为Encoding.UTF8
        /// </summary>
        public static string ToHexString(this string source, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] bytes = encoding.GetBytes(source);
            return bytes.ToHexString();
        }

        /// <summary>
        /// 将十六进制字符串转换为常规字符串，默认编码为Encoding.UTF8
        /// </summary>
        public static string FromHexString(this string hexString, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] bytes = hexString.ToHexBytes();
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 将byte[]编码为十六进制字符串
        /// </summary>
        /// <param name="bytes">byte[]数组</param>
        /// <returns>十六进制字符串</returns>
        public static string ToHexString(this byte[] bytes)
        {
            return bytes.Aggregate(string.Empty, (current, t) => current + t.ToString("X2"));
        }

        /// <summary>
        /// 将十六进制字符串转换为byte[]
        /// </summary>
        /// <param name="hexString">十六进制字符串</param>
        /// <returns>byte[]数组</returns>
        public static byte[] ToHexBytes(this string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if (hexString.Length % 2 != 0)
            {
                hexString = hexString ?? "";
            }
            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        /// <summary>
        /// 将字符串进行Unicode编码，变成形如“\u7f16\u7801”的形式
        /// </summary>
        /// <param name="source">要进行编号的字符串</param>
        public static string ToUnicodeString(this string source)
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
                str = char.ToLowerInvariant(str[0]) + str.Substring(1, str.Length - 1);
            }
            return words.ExpandAndToString(splitStr);
        }

        /// <summary>
        /// 将驼峰字符串按单词拆分并转换成大写，再以特定字符串分隔
        /// </summary>
        /// <param name="str">待转换的字符串</param>
        /// <param name="splitStr">分隔符字符</param>
        /// <returns></returns>
        public static string LowerToUpperAndSplit(this string str, string splitStr = "-")
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
                    str = str.UpperFirstChar();
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
                str = char.ToUpperInvariant(str[0]) + str[1..];
            }
            return words.ExpandAndToString(splitStr);
        }

        /// <summary>
        /// 将第一个字符小写
        /// </summary>
        public static string LowerFirstChar(this string str)
        {
            if (string.IsNullOrEmpty(str) || !char.IsUpper(str[0]))
            {
                return str;
            }
            if (char.IsUpper(str[0]))
            {
                if (str.Length == 1)
                {
                    return char.ToLowerInvariant(str[0]).ToString();
                }

                return char.ToLowerInvariant(str[0]) + str[1..];
            }
            return str;
        }

        /// <summary>
        /// 将第一个字符大写
        /// </summary>
        public static string UpperFirstChar(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (char.IsLower(str[0]))
            {
                if (str.Length == 1)
                {
                    return char.ToUpperInvariant(str[0]).ToString();
                }

                return char.ToUpperInvariant(str[0]) + str[1..];
            }
            return str;
        }

        /// <summary>
        /// 将字符串转化为符合大驼峰命名的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUpperCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            var splitStr = str.Split('_');
            var list = new List<string>(splitStr.Length);
            foreach (var s in splitStr)
            {
                list.Add(s.UpperFirstChar());
            }

            return string.Join("", list);
        }

        /// <summary>
        /// 将字符串转化为符合小驼峰命名的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToLowerCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            var splitStr = str.Split('_');
            var list = new List<string>(splitStr.Length);
            foreach (var s in splitStr)
            {
                list.Add(s.LowerFirstChar());
            }

            return string.Join("", list);
        }

        /// <summary>
        /// 将字符串转化为符合蛇形命名的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToSnakeCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (str.FirstOrDefault(char.IsUpper) == default)
            {
                return str;
            }

            var list = new List<char>();
            for (var i = 0; i < str.Length; i++)
            {
                var c = str[i];
                if (char.IsUpper(c))
                {
                    if (i == 0)
                    {
                        list.Add(char.ToLowerInvariant(c));
                    }
                    else
                    {
                        list.Add('_');
                        list.Add(char.ToLowerInvariant(c));
                    }
                }
                else
                {
                    list.Add(c);
                }
            }

            return new string(list.ToArray());
        }

        /// <summary>
        /// 将字符串转换成指定类型，如果转换失败则返回默认值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object TryConvertType(this string str, Type conversionType)
        {
            try
            {
                if (str == null)
                {
                    return default;
                }
                if (conversionType.IsNullableType())
                {
                    conversionType = conversionType.GetUnNullableType();
                }
                if(conversionType == typeof(string))
                {
                    return str;
                }
                if (conversionType.IsEnum)
                {
                    return Enum.Parse(conversionType, str);
                }
                if (conversionType == typeof(Guid))
                {
                    return Guid.Parse(str);
                }
                return TypeDescriptor.GetConverter(conversionType).ConvertFromInvariantString(str);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// 字符串左取处理
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">左取长度</param>
        /// <returns></returns>
        public static string SubstringLeft(this string str, int length)
        {
            if (str.Length < length)
            {
                return str;
            }
            return str.Substring(0, length);
        }

        /// <summary>
        /// 字符串右取处理
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">右取长度</param>
        /// <returns></returns>
        public static string SubstringRight(this string str, int length)
        {
            if (str.Length < length)
            {
                return str;
            }
            return str.Substring(str.Length - length, length);
        }

        /// <summary>
        /// 字符串补充字符串和左取处理
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">左取长度</param>
        /// <param name="paddingChar">补充字符</param>
        /// <returns></returns>
        public static string SubstringPadLeft(this string str, int length, char paddingChar)
        {
            return str.PadLeft(length, paddingChar).SubstringLeft(length);
        }

        /// <summary>
        /// 字符串补充字符串和右取处理
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">右取长度</param>
        /// <param name="paddingChar">补充字符</param>
        /// <returns></returns>
        public static string SubstringPadRight(this string str, int length, char paddingChar)
        {
            return str.PadRight(length, paddingChar).SubstringRight(length);
        }
    }
}
