using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Rye.Util
{
    /// <summary>
    /// 提供对RFC3986协议的编码支持
    /// </summary>
    public class RFC3986EncoderUtil
    {
        public static string Encode(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder newStr = new StringBuilder();

            foreach (var item in input)
            {
                if (IsReverseChar(item))
                {
                    newStr.Append("%");
                    var temp = ((int)item).ToString("X2");
                    newStr.Append(temp);
                }
                else
                    newStr.Append(item);
            }

            return newStr.ToString();
        }

        /// <summary>
        /// 根据RFC3986协议进行UrlEncode，对应php中的rawurlencode
        /// </summary>
        /// <param name="strSrc"></param>
        /// <param name="encoding"></param>
        /// <param name="bToUpper"></param>
        /// <returns></returns>
        public static string UrlEncode(string strSrc, System.Text.Encoding encoding, bool bToUpper = true)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            for (int i = 0; i < strSrc.Length; i++)
            {
                string t = strSrc[i].ToString();
                string k = HttpUtility.UrlEncode(t, encoding);
                if (t == k)
                {
                    stringBuilder.Append(t);
                }
                else
                {
                    if (bToUpper)
                        stringBuilder.Append(k.ToUpper());
                    else
                        stringBuilder.Append(k);
                }
            }
            return stringBuilder.ToString();
        }

        private static bool IsReverseChar(char c)
        {
            return !((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9')
                    || c == '-' || c == '_' || c == '.' || c == '~');
        }
    }
}
