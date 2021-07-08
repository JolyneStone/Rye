using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Rye
{
    public static class SerializerExtensions
    {
        static SerializerExtensions()
        {
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                //ReferenceHandler = ReferenceHandler.Preserve,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            };

            _serializerOptions.Converters.Add(new DateTimeJsonConverter());
        }

        private static readonly JsonSerializerOptions _serializerOptions;
        private static readonly Type StringType = typeof(string);
        public static string ToJsonString(this object obj, JsonSerializerOptions options = null)
        {
            if (obj == null)
                return null;
            if (obj is string str)
                return str;
            if (options == null)
            {
                options = _serializerOptions;
            }
            return JsonSerializer.Serialize(obj, options);
        }

        /// <summary>
        /// 将Json字符串转化为对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static object ToObject(this string value, JsonSerializerOptions options = null)
        {
            if (options == null)
            {
                options = _serializerOptions;
            }

            return JsonSerializer.Deserialize<object>(value, options);
        }

        /// <summary>
        /// 将Json字符串转化为指定类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string value, JsonSerializerOptions options = null)
        {
            if (options == null)
            {
                options = _serializerOptions;
            }

            var type = typeof(T);
            if (type.IsPrimitive)
                return (T)value.Parse(type);
            if (type == StringType)
                return (T)(object)value;

            return JsonSerializer.Deserialize<T>(value, options);
        }

        /// <summary>
        /// 将Json字符串转化为指定类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static object ToObject(this string value, Type type, JsonSerializerOptions options = null)
        {
            if (options == null)
            {
                options = _serializerOptions;
            }

            if (type.IsPrimitive)
                return value.Parse(type);
            if (type == StringType)
                return value;

            return JsonSerializer.Deserialize(value, type, options);
        }
    }
}
