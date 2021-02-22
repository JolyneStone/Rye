using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rye
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 当前时间是否周末
        /// </summary>
        /// <param name="dateTime">时间点</param>
        /// <returns></returns>
        public static bool IsWeekend(this DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// 当前时间是否工作日
        /// </summary>
        /// <param name="dateTime">时间点</param>
        /// <returns></returns>
        public static bool IsWeekday(this DateTime dateTime)
        {
            return IsWeekend(dateTime);
        }

        /// <summary>
        /// 获取时间相对唯一字符串
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="milsec">是否使用毫秒</param>
        /// <returns></returns>
        public static string ToUniqueString(this DateTime dateTime, bool milsec = false)
        {
            int seconds = dateTime.Hour * 3600 + dateTime.Minute * 60 + dateTime.Second;
            string value = $"{dateTime:yy}{dateTime.DayOfYear}{seconds}";
            return milsec ? value + dateTime.ToString("fff") : value;
        }

        /// <summary>
        /// 将当前时区时间转换为UTC时间
        /// </summary>
        public static DateTime ToUtcTime(this DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, TimeZoneInfo.Local);
        }

        /// <summary>
        /// 将指定UTC时间转换为当前时区的时间
        /// </summary>
        public static DateTime FromUtcTime(this DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.Local);
        }

        /// <summary>
        /// 将时间转换为JS时间格式(Date.getTime())
        /// </summary>
        public static string ToJsGetTime(this DateTime dateTime, bool milsec = true)
        {
            DateTime utc = dateTime.ToUniversalTime();
            TimeSpan span = utc.Subtract(new DateTime(1970, 1, 1));
            return Math.Round(milsec ? span.TotalMilliseconds : span.TotalSeconds).ToString();
        }

        /// <summary>
        /// 将JS时间格式的数值转换为时间
        /// </summary>
        public static DateTime FromJsGetTime(this long jsTime)
        {
            int length = jsTime.ToString().Length;
            Check.Required<ArgumentException>(length != 10 || length != 13, "JS时间数值的长度不正确，必须为10位或13位");
            DateTime start = new DateTime(1970, 1, 1);
            DateTime result = length == 10 ? start.AddSeconds(jsTime) : start.AddMilliseconds(jsTime);
            return result.FromUtcTime();
        }

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
        /// 返回UTC时间
        /// </summary>
        /// <param name="timesTamp"></param>
        /// <returns></returns>
        public static DateTime ParseMilliUnixDateTimes(this long timesTamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(timesTamp);
        }

        /// <summary>
        /// 返回UTC时间
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
        ///由秒数得到日期几天几小时..
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
