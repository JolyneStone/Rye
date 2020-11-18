using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Monica
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
                ReferenceHandler = ReferenceHandler.Preserve,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            };

            _serializerOptions.Converters.Add(new DateTimeJsonConverter());
        }

        private static readonly JsonSerializerOptions _serializerOptions;
        public static string ToJsonString(this object obj, JsonSerializerOptions options = null)
        {
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
            if (options != null)
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
            if (options != null)
            {
                options = _serializerOptions;
            }

            return JsonSerializer.Deserialize<T>(value, options);
        }
    }
}
