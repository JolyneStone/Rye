using Newtonsoft.Json;

namespace Kira.AlasFx
{
    public static class StringExtensions
    {
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
    }
}
