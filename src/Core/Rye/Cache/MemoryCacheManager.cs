using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Rye.DependencyInjection;
using System;

namespace Rye.Cache
{
    public class MemoryCacheManager
    {
        private static readonly IMemoryCache _cache = SingleServiceLocator.ServiceProvider.GetRequiredService<IMemoryCache>();

        public static IMemoryCache Cache
        {
            get
            {
                return _cache;
            }
        }

        public static void Set(string key, object value, double duration = 0)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (duration == 0)
                    _cache.Set(key, value); //假如不传值就缓存永久
                else
                    _cache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(duration)));
            }
        }

        public static object Get(string key)
        {
            if (key != null && _cache.TryGetValue(key, out object val))
            {
                return val;
            }
            else
            {
                return null;
            }
        }

        public static T Get<T>(string key)
        {
            var val = Get(key);
            if (val != null)
            {
                return (T)val;
            }
            return default;
        }

        public static void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
