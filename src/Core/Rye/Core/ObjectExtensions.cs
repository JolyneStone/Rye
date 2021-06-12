using Rye;
using System;
using System.Collections.Generic;
using System.Text;
using Rye.Core;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rye
{
    /// <summary>
    /// Object扩展方法
    /// </summary>
    public static class ObjectExtensions
    {
        #region Parse
        public static short ParseByInt16(this object obj)
        {
            short temp = default;
            if (obj == null)
            {
                return temp;
            }

            return short.TryParse(obj.ToString(), out temp) ? temp : default;
        }
        public static int ParseByInt(this object obj)
        {
            int temp = default;
            if (obj == null)
            {
                return temp;
            }

            return int.TryParse(obj.ToString(), out temp) ? temp : default;
        }
        public static string ParseByString(this object obj)
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

        public static int ParseByIntMax(this object obj)
        {
            int temp = int.MaxValue;
            if (obj == null)
            {
                return temp;
            }

            return int.TryParse(obj.ToString(), out temp) ? temp : int.MaxValue;
        }
        public static short ParseByShort(this object obj)
        {
            short temp = default;
            if (obj == null)
            {
                return temp;
            }

            return short.TryParse(obj.ToString(), out temp) ? temp : default;
        }
        public static long ParseByLong(this object obj)
        {
            long temp = default;
            if (obj == null)
            {
                return temp;
            }

            return long.TryParse(obj.ToString(), out temp) ? temp : default;
        }


        public static uint ParseByUInt32(this object obj)
        {
            uint temp = default;
            if (obj == null)
            {
                return temp;
            }

            return uint.TryParse(obj.ToString(), out temp) ? temp : default;
        }
        public static ushort ParseByUInt16(this object obj)
        {
            ushort temp = default;
            if (obj == null)
            {
                return temp;
            }

            return ushort.TryParse(obj.ToString(), out temp) ? temp : default;
        }
        public static ulong ParseByUlong(this object obj)
        {
            ulong temp = default;
            if (obj == null)
            {
                return temp;
            }

            return ulong.TryParse(obj.ToString(), out temp) ? temp : default;
        }

        public static decimal ParseByDecimal(this object obj)
        {
            decimal temp = default;
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
                return default;
            }
            else
            {
                return decimal.TryParse(obj.ToString(), out temp) ? temp : default;
            }
        }

        public static decimal ParseByDecimal(this object obj, int precision)
        {
            return Math.Round(ParseByDecimal(obj), precision);
        }

        public static float ParseByFloat(this object obj)
        {
            float temp = default;
            if (obj == null)
            {
                return temp;
            }

            return float.TryParse(obj.ToString(), out temp) ? temp : default;
        }
        public static double ParseByDouble(this object obj)
        {
            double temp = default;
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
                return default;
            }
            else
            {
                return double.TryParse(obj.ToString(), out temp) ? temp : default;
            }
        }

        public static TEnum ParseByEnum<TEnum>(this object obj, TEnum defaultValue = default)
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

        public static bool ParseByBool(this object obj)
        {
            bool temp = default;
            if (obj == null)
            {
                return temp;
            }

            return bool.TryParse(obj.ToString(), out temp) ? temp : default;
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

        /// <summary>
        /// obj.ToString之后DateTime.Parse
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime ParseByDateTime(this object obj)
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
        /// obj.ToString之后DateTime.Parse带格式
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime ParseExactByDateTime(this object obj, string format)
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

        public static T Parse<T>(this object obj, T defaultValue = default)
        {
            try
            {
                if (obj == null && default(T) == null)
                {
                    return defaultValue;
                }
                if (obj.GetType() == typeof(T))
                {
                    return (T)obj;
                }
                object result = Parse(obj, typeof(T));
                return (T)result;
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static object Parse(this object value, Type conversionType)
        {
            try
            {
                if (value == null)
                {
                    return conversionType.IsValueType ? Activator.CreateInstance(conversionType) : null;
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
                return conversionType.IsValueType ? Activator.CreateInstance(conversionType) : null;
            }
        }

        #endregion

        #region 四舍五入
        /// <summary>
        /// 转为小数，默认为8位，最多20位小数，截断
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDigitString(this decimal value, int position = 8)
        {
            var tempStr = value.ToString("0.####################");

            //有多少位小数
            int digitlength = tempStr.Length - tempStr.IndexOf('.') - 1;

            //小数位大于digits(默认8)
            if (tempStr.IndexOf('.') > 0 && digitlength > position)
            {
                tempStr = tempStr.Substring(0, tempStr.Length - (digitlength - position));

                return Convert.ToDecimal(tempStr) == 0 ? "0" : tempStr;
            }
            else
            {
                return tempStr;
            }
        }

        /// <summary>
        /// 转为小数，默认为8位，最多20位小数，截断
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDigitString(this double value, int position = 8)
        {
            var tempStr = value.ToString("0.####################");

            //有多少位小数
            int digitlength = tempStr.Length - tempStr.IndexOf('.') - 1;

            //小数位大于digits(默认8)
            if (tempStr.IndexOf('.') > 0 && digitlength > position)
            {
                tempStr = tempStr.Substring(0, tempStr.Length - (digitlength - position));

                return Convert.ToDecimal(tempStr) == 0 ? "0" : tempStr;
            }
            else
            {
                return tempStr;
            }
        }

        /// <summary>
        /// 转为小数，默认为8位，最多20位小数，截断
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDigitString(this float value, int position = 8)
        {
            var tempStr = value.ToString("0.####################");

            //有多少位小数
            int digitlength = tempStr.Length - tempStr.IndexOf('.') - 1;

            //小数位大于digits(默认8)
            if (tempStr.IndexOf('.') > 0 && digitlength > position)
            {
                tempStr = tempStr.Substring(0, tempStr.Length - (digitlength - position));

                return Convert.ToDecimal(tempStr) == 0 ? "0" : tempStr;
            }
            else
            {
                return tempStr;
            }
        }

        /// <summary>
        /// 转为小数，默认为8位，四舍五入
        /// </summary>
        /// <param name="value"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static string ToDigitStringRound(this decimal value, int position = 8)
        {
            if (value == 0)
            {
                return "0";
            }

            string str = ".";
            for (int i = 0; i < position; i++)
            {
                str += "#";
            }
            if (value > 0 && value < 1)
            {
                return "0" + value.ToString(str);
            }
            else if (value < 0 && value > -1)
            {
                return "-0" + (-value).ToString(str);
            }
            else
            {
                return value.ToString(str);
            }
        }


        /// <summary>
        /// 保留position位有效小数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static string ToDigitStringRound(this double value, int position = 8)
        {
            if (value == 0)
            {
                return "0";
            }

            string str = ".";
            for (int i = 0; i < position; i++)
            {
                str += "#";
            }
            if (value > 0 && value < 1)
            {
                return "0" + value.ToString(str);
            }
            else if (value < 0 && value > -1)
            {
                return "-0" + (-value).ToString(str);
            }
            else
            {
                return value.ToString(str);
            }
        }


        /// <summary>
        /// 保留position位有效小数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static string ToDigitStringRound(this float value, int position = 8)
        {
            if (value == 0)
            {
                return "0";
            }

            string str = ".";
            for (int i = 0; i < position; i++)
            {
                str += "#";
            }
            if (value > 0 && value < 1)
            {
                return "0" + value.ToString(str);
            }
            else if (value < 0 && value > -1)
            {
                return "-0" + (-value).ToString(str);
            }
            else
            {
                return value.ToString(str);
            }
        }
        #endregion

        #region Clone

        public static TSource Clone<TSource>(this TSource source) where TSource : new()
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
