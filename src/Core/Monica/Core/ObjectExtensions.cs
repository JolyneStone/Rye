using Monica.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using Monica.Core;

namespace Monica
{
    /// <summary>
    /// Object扩展方法
    /// </summary>
    public static class ObjectExtensions
    {
        #region 序列化
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

        #endregion

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

        public static TEnum TryParseByEnum<TEnum>(this object obj, TEnum defaultValue = default)
            where TEnum : Enum
        {
            if (obj == null && default(TEnum) == null)
            {
                return defaultValue;
            }
            var type = typeof(TEnum);
            if (obj.GetType() == type)
            {
                return (TEnum)obj;
            }

            return (TEnum)Enum.Parse(type, obj.ToString());
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

        public static object TryPase(this object value, Type conversionType)
        {
            try
            {
                if (value == null)
                {
                    return null;
                }
                if (conversionType.IsNullableType())
                {
                    conversionType = conversionType.GetUnNullableType();
                }
                if (conversionType.IsEnum)
                {
                    return Enum.Parse(conversionType, value.ToString());
                }
                if (conversionType == typeof(Guid))
                {
                    return Guid.Parse(value.ToString());
                }
                return Convert.ChangeType(value, conversionType);
            }
            catch
            {
                return default;
            }
        }

        public static T TryParse<T>(this object obj, T defaultValue = default)
        {
            try
            {
                if (obj == null && default(T) == null)
                {
                    return default;
                }
                if (obj.GetType() == typeof(T))
                {
                    return (T)obj;
                }
                object result = TryParse(obj, typeof(T));
                return (T)result;
            }
            catch (Exception)
            {
                return defaultValue;
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

        #endregion

        #region Clone

        public static TSource Clone<TSource>(this TSource source) where TSource: new()
        {
            return CloneObject<TSource, TSource>.Clone(source);
        }

        public static TTArget Clone<TSource, TTArget>(this TSource source) where TTArget : TSource, new()
        {
            return CloneObject<TSource, TTArget>.Clone(source);
        }

        public static void Clone<TSource, TTArget>(this TSource source, TTArget target) where TTArget : TSource
        {
            CloneObject<TSource, TTArget>.Clone(source, target);
        }

        #endregion
    }
}
