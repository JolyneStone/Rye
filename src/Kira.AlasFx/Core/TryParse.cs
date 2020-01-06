using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.AlasFx
{
    public static class TryParse
    {
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
    }
    public static class Unix
    {
        /// <summary>
        /// 适合所有时区的输入
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static int ParseUnixTimesTamp(this DateTime datetime)
        {
            //double ticks = (datetime.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            //if (ticks > int.MaxValue)
            //    return Convert.ToInt32((ticks / 1000).ToString("0"));
            //else
            //    return Convert.ToInt32(ticks.ToString("0"));
            datetime = datetime.ToUniversalTime();
            DateTime utcDate = new DateTime(1970, 1, 1, 0, 0, 0);
            return (int)(datetime - utcDate).TotalSeconds;
        }

        /// <summary>
        /// 适合所有时区的输入
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static long ParseMilliUnixTimesTamp(this DateTime datetime)
        {
            datetime = datetime.ToUniversalTime();
            DateTime utcDate = new DateTime(1970, 1, 1, 0, 0, 0);
            return (long)((datetime - utcDate).TotalMilliseconds);
        }

        /// <summary>
        /// 必须输入utc时间
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static int ParseTimesTamp(this DateTime datetime)
        {
            DateTime utcDate = new DateTime(1970, 1, 1, 0, 0, 0);
            return (int)(datetime - utcDate).TotalSeconds;
        }

        /// <summary>
        /// 必须输入utc时间
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static long ParseMilliTimesTamp(this DateTime datetime)
        {
            DateTime utcDate = new DateTime(1970, 1, 1, 0, 0, 0);
            return (long)(datetime - utcDate).TotalMilliseconds;
        }

        /// <summary>
        /// 必须输入utc时间
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static double ParseMilliTimesTampOffSet(this DateTime datetime)
        {
            DateTime utcDate = new DateTime(1970, 1, 1, 0, 0, 0);
            return (datetime - utcDate).TotalMilliseconds;
        }
        /// <summary>
        /// 纳秒，必须输入utc时间
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static long ParseNsTimesTamp(this DateTime datetime)
        {
            DateTime utcDate = new DateTime(1970, 1, 1, 0, 0, 0);
            TimeSpan ts = (datetime - utcDate);
            long totalmilliseconds = (long)ts.TotalMilliseconds;     //TotalMilliseconds会丢失纳秒级别的精度
            long nsUnderMs = (ts.Ticks % 10000) * 100;
            long s = totalmilliseconds * 1000000 + nsUnderMs;
            return s;
        }

        /// <summary>
        /// 返回UTC时间。
        /// </summary>
        /// <param name="timesTamp"></param>
        /// <returns></returns>
        public static DateTime ParseMilliUnixDateTimes(this long timesTamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(timesTamp);
        }

        /// <summary>
        /// 返回UTC时间。
        /// </summary>
        /// <param name="timesTamp"></param>
        /// <returns></returns>
        public static DateTime ParseMilliUnixDateTimes(this double timesTamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(timesTamp);
        }

        public static DateTime ParseBeijingDateTimes(this int timesTamp)
        {
            return new DateTime(1970, 1, 1, 8, 0, 0).AddSeconds(timesTamp);
        }

        /// <summary>
        /// 返回东八时区的DateTime
        /// </summary>
        /// <param name="timesTamp"></param>
        /// <returns></returns>
        public static DateTime ParseMilliBeijingDateTimes(this long timesTamp)
        {
            return new DateTime(1970, 1, 1, 8, 0, 0).AddMilliseconds(timesTamp);
        }

        /// <summary>
        /// 根据hour返回哪个时区的datetime
        /// </summary>
        /// <param name="timesTamp"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static DateTime ParseMilliDateTimes(this long timesTamp, int hours = 0)
        {
            return new DateTime(1970, 1, 1, hours, 0, 0).AddMilliseconds(timesTamp);
        }

        /// <summary>
        /// 返回utc时间
        /// </summary>
        /// <param name="timesTamp"></param>
        /// <returns></returns>
        public static DateTime ParseUnixDateTimes(this int timesTamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(timesTamp);
        }

        ///<summary>
        ///由秒数得到日期几天几小时...
        ///</summary
        ///<param name="totalSecond">秒数</param>
        ///<returns>几天几小时几分几秒</returns>
        public static string ParseTimeSeconds(this long totalSecond)
        {
            string r = "";
            int day, hour, minute, second;

            if (totalSecond >= 86400) //天,
            {
                day = Convert.ToInt16(totalSecond / 86400);
                hour = Convert.ToInt16((totalSecond % 86400) / 3600);
                minute = Convert.ToInt16((totalSecond % 86400 % 3600) / 60);
                second = Convert.ToInt16(totalSecond % 86400 % 3600 % 60);
                r = $"{day}天{hour}时{minute}分{second}秒";
            }
            else if (totalSecond >= 3600)//时,
            {
                hour = Convert.ToInt16(totalSecond / 3600);
                minute = Convert.ToInt16((totalSecond % 3600) / 60);
                second = Convert.ToInt16(totalSecond % 3600 % 60);
                r = $"{hour}时{minute}分{second}秒";
            }
            else if (totalSecond >= 60)//分
            {
                minute = Convert.ToInt16(totalSecond / 60);
                second = Convert.ToInt16(totalSecond % 60);
                r = $"{minute}分{second}秒";
            }
            else
            {
                second = Convert.ToInt16(totalSecond);
                r = $"{second}秒";
            }
            return r;
        }
    }
}
