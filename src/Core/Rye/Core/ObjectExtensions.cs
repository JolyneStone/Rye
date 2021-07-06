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
        /// <summary>
        /// 将对象转化为int类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ParseByInt(this object obj, int defaultValue = default)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            return int.TryParse(obj.ToString(), out var temp) ? temp : defaultValue;
        }

        /// <summary>
        /// 将对象转化为string类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string ParseByString(this object obj, string defaultValue = default)
        {
            if (obj == null)
            {
                return defaultValue;
            }
            else
            {
                return obj.ToString();
            }
        }

        /// <summary>
        /// 将对象转化为short类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static short ParseByShort(this object obj, short defaultValue = default)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            return short.TryParse(obj.ToString(), out var temp) ? temp : defaultValue;
        }

        /// <summary>
        /// 将对象转化为byte类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static byte ParseByByte(this object obj, byte defaultValue = default)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            return byte.TryParse(obj.ToString(), out var temp) ? temp : defaultValue;
        }

        /// <summary>
        /// 将对象转化为long类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ParseByLong(this object obj, long defaultValue = default)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            return long.TryParse(obj.ToString(), out var temp) ? temp : defaultValue;
        }

        /// <summary>
        /// 将对象转化为uint
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static uint ParseByUInt(this object obj, uint defaultValue = default)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            return uint.TryParse(obj.ToString(), out var temp) ? temp : defaultValue;
        }

        /// <summary>
        /// 将对象转化为ushort
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static ushort ParseByUShort(this object obj, ushort defaultValue = default)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            return ushort.TryParse(obj.ToString(), out var temp) ? temp : defaultValue;
        }

        /// <summary>
        /// 将对象转化为ulong
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static ulong ParseByULong(this object obj, ulong defaultValue = default)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            return ulong.TryParse(obj.ToString(), out var temp) ? temp : defaultValue;
        }

        /// <summary>
        /// 将对象转化为decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ParseByDecimal(this object obj, decimal defaultValue = default)
        {
            if (obj == null)
            {
                return defaultValue;
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
                return decimal.TryParse(obj.ToString(), out var temp) ? temp : defaultValue;
            }
        }

        /// <summary>
        /// 将对象转化为decimal, 并保留precision位小数
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="precision"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ParseByDecimal(this object obj, int precision, decimal defaultValue = default)
        {
            return Math.Round(ParseByDecimal(obj, defaultValue), precision, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 将对象转化为float
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float ParseByFloat(this object obj, float defaultValue = default)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            return float.TryParse(obj.ToString(), out var temp) ? temp : defaultValue;
        }

        /// <summary>
        /// 将对象转化为double
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ParseByDouble(this object obj, double defaultValue = default)
        {
            if (obj == null)
            {
                return defaultValue;
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
                return double.TryParse(obj.ToString(), out var temp) ? temp : defaultValue;
            }
        }

        /// <summary>
        /// 将对象转化为double, 并保留precision位小数
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="precision"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ParseByDouble(this object obj, int precision, double defaultValue = default)
        {
            return Math.Round(ParseByDouble(obj, defaultValue), precision, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 将对象转化为Guid
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Guid ParseByGuid(this object obj, Guid defaultValue = default)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            return Guid.TryParse(obj.ToString(), out var temp) ? temp : defaultValue;
        }

        /// <summary>
        /// 将对象转化为指定的枚举
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 将对象转化为bool
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool ParseByBool(this object obj, bool defaultValue = default)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            return bool.TryParse(obj.ToString(), out var temp) ? temp : defaultValue;
        }

        /// <summary>
        /// 将对象转化为任意类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object Parse(this object obj, Type conversionType)
        {
            try
            {
                if (obj == null)
                {
                    return conversionType.IsValueType ? Activator.CreateInstance(conversionType) : null;
                }
                if (conversionType.IsNullableType())
                {
                    conversionType = conversionType.GetUnNullableType();
                }
                if (conversionType.IsEnum)
                {
                    return Enum.Parse(conversionType, obj.ToString());
                }
                if (conversionType == typeof(Guid))
                {
                    return Guid.Parse(obj.ToString());
                }
                return Convert.ChangeType(obj, conversionType);
            }
            catch
            {
                return conversionType.IsValueType ? Activator.CreateInstance(conversionType) : null;
            }
        }

        /// <summary>
        /// 将对象转化为任意类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
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
                return defaultValue;
            }
        }

        /// <summary>
        /// 将对象转化为DateTime
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ParseByDateTime(this object obj, DateTime defaultValue = default)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            if (DateTime.TryParse(obj.ToString(), out var val))
            {
                return val;
            }
            return defaultValue;
        }

        /// <summary>
        /// 将对象转化为DateTime
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="format"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ParseExactByDateTime(this object obj, string format, DateTime defaultValue = default)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            try
            {
                return DateTime.ParseExact(obj.ToString(), format, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch { }
            return defaultValue;
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
