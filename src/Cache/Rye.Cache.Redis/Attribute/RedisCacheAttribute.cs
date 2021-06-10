using Microsoft.Extensions.DependencyInjection;

using Rye.AspectFlare;
using Rye.Cache.Redis.Store;

using System;
using System.ComponentModel;
using System.Text;

namespace Rye.Cache.Redis
{
    /// <summary>
    /// 提供方法数据缓存功能，使用IRedisStore
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RedisCacheAttribute : InterceptAttribute, ICallingInterceptor, ICalledInterceptor
    {
        private static readonly Lazy<IRedisStore> _cacheLazy = new Lazy<IRedisStore>(() => App.ApplicationServices.GetService<IRedisStore>());

        private static IRedisStore Cache { get => _cacheLazy.Value; }

        public RedisCacheAttribute()
        {
        }

        public RedisCacheAttribute(string cacheKey)
        {
            Check.NotNullOrEmpty(cacheKey, nameof(cacheKey));
            CacheKey = cacheKey;
        }

        public RedisCacheAttribute(string cacheKey, int cacheSeconds)
        {
            Check.NotNullOrEmpty(cacheKey, nameof(cacheKey));
            CacheKey = cacheKey;
            CacheSeconds = cacheSeconds;
        }

        public RedisCacheAttribute(string cacheKey, int cacheSeconds, bool isSliding)
        {
            Check.NotNullOrEmpty(cacheKey, nameof(cacheKey));

            CacheKey = cacheKey;
            CacheSeconds = cacheSeconds;
            IsSliding = isSliding;
        }

        public RedisCacheAttribute(int cacheSeconds)
        {
            CacheSeconds = cacheSeconds;
        }

        public RedisCacheAttribute(int cacheSeconds, bool isSliding)
        {
            CacheSeconds = cacheSeconds;
            IsSliding = isSliding;
        }

        /// <summary>
        /// 缓存Key
        /// </summary>
        public string CacheKey { get; set; }
        public int CacheSeconds { get; set; } = 30;
        public bool IsSliding { get; set; } = false;

        public void Calling(CallingInterceptContext callingInterceptorContext)
        {
            if (string.IsNullOrEmpty(CacheKey))
            {
                CacheKey = callingInterceptorContext.Owner.GetType().FullName + "." + callingInterceptorContext.MethodName;
            }
            var key = GetKey(CacheKey, callingInterceptorContext.Parameters);
            var result = Cache.Get<string>(new CacheOptionEntry
            {
                Key = key,
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(CacheSeconds)
            });
            if (result != null)
            {
                callingInterceptorContext.HasResult = true;
                callingInterceptorContext.Result = callingInterceptorContext.ReturnType.GetUnNullableType().IsPrimitive ?
                    TypeDescriptor.GetConverter(callingInterceptorContext.ReturnType).ConvertFromInvariantString(result) :
                    (callingInterceptorContext.ReturnType == typeof(string) ?
                    result :
                    result.ToObject(callingInterceptorContext.ReturnType));
            }
        }

        public void Called(CalledInterceptContext calledInterceptorContext)
        {
            var key = GetKey(CacheKey, calledInterceptorContext.Parameters);
            if (!Cache.Exist(key))
            {
                var result = calledInterceptorContext.Result ?? calledInterceptorContext.ReturnValue;
                if (result != null)
                {
                    CacheOptionEntry entry = new CacheOptionEntry();
                    entry.Key = key;
                    if (IsSliding)
                    {
                        entry.SetSlidingExpiration(TimeSpan.FromSeconds(CacheSeconds));
                    }
                    else
                    {
                        entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheSeconds));
                    }
                    Cache.Set(entry, result.ToJsonString());
                }
            }
        }

        private static string GetKey(string cacheKey, object[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                var sb = new StringBuilder(cacheKey);
                foreach (var param in parameters)
                {
                    sb.Append("_" + param?.GetHashCode());
                }

                return sb.ToString();
            }
            else
            {
                return cacheKey;
            }
        }
    }
}
