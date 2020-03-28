using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace KiraNet.AlasFx
{
    /// <summary>
    /// Object扩展方法
    /// </summary>
    public static class ObjectExtensions
    {
        public static string ToJsonString(this object obj, bool camelCase = false, bool indented = false)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            if (camelCase)
            {
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
            if (indented)
            {
                settings.Formatting = Formatting.Indented;
            }
            return JsonConvert.SerializeObject(obj, settings);
        }

        #region TryParse
        public static short TryParseByInt16(this object obj, short defaultValue = 0)
        {
            short temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            return short.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
        }
        public static int TryParseByInt(this object obj, int defaultValue = 0)
        {
            int temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            return int.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
        }
        public static string TryParseByString(this object obj, string defaultValue = "")
        {
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return obj.ToString();
            }
        }

        public static int TryParseByIntMax(this object obj)
        {
            int temp = int.MaxValue;
            if (obj == null)
            {
                return temp;
            }

            return int.TryParse(obj.ToString(), out temp) ? temp : int.MaxValue;
        }
        public static short TryParseByShort(this object obj, short defaultValue = 0)
        {
            short temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            return short.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
        }
        public static long TryParseByLong(this object obj, long defaultValue = 0)
        {
            long temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            return long.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
        }


        public static uint TryParseByUInt32(this object obj, uint defaultValue = 0)
        {
            uint temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            uint.TryParse(obj.ToString(), out temp);
            return uint.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
        }
        public static ushort TryParseByUInt16(this object obj, ushort defaultValue = 0)
        {
            ushort temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            return ushort.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
        }
        public static ulong TryParseByUlong(this object obj, ulong defaultValue = 0)
        {
            ulong temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            return ulong.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
        }

        public static decimal TryParseByDecimal(this object obj, decimal defaultValue = 0)
        {
            decimal temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            string _decimalContent = obj.ToString();
            if (_decimalContent.Contains("E"))
            {
                try
                {
                    return Convert.ToDecimal(decimal.Parse(_decimalContent, System.Globalization.NumberStyles.Float));
                }
                catch
                {

                }
                return defaultValue;
            }
            else
            {
                return decimal.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
            }
        }

        public static decimal TryParseByDecimal(this object obj, int precision)
        {
            return Math.Round(TryParseByDecimal(obj, 0m), precision);
        }

        public static float TryParseByFloat(this object obj, float defaultValue = 0)
        {
            float temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            return float.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
        }
        public static double TryParseByDouble(this object obj, double defaultValue = 0)
        {
            double temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            string _doubleContent = obj.ToString();
            if (_doubleContent.Contains("E"))
            {
                try
                {
                    return Convert.ToDouble(double.Parse(_doubleContent, System.Globalization.NumberStyles.Float));
                }
                catch
                {

                }
                return defaultValue;
            }
            else
            {
                return double.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
            }



        }

        /// <summary>
        /// obj.ToString之后DateTime.TryParse
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime TryParseByDateTime(this object obj)
        {
            DateTime temp = DateTime.MinValue;
            if (obj == null)
            {
                return temp;
            }

            if (DateTime.TryParse(obj.ToString(), out temp))
            {
                return Convert.ToDateTime(obj);
            }
            return temp;
        }

        /// <summary>
        /// obj.ToString之后DateTime.TryParse带格式
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime TryParseExactByDateTime(this object obj, string format)
        {
            DateTime temp = DateTime.MinValue;
            if (obj == null)
            {
                return temp;
            }

            try
            {
                return DateTime.ParseExact(obj.ToString(), format, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch { }
            return temp;
        }
        public static bool TryParseByBool(this object obj, bool defaultValue = false)
        {
            bool temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            return bool.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
        }
        #endregion
    }
}
