using Microsoft.Extensions.Caching.Distributed;
using Monica.AspectFlare;
using Monica.DependencyInjection;
using System;

namespace Monica.Cache
{
    /// <summary>
    /// 提供方法数据缓存功能，使用IDistributedCache
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CacheAttribute : InterceptAttribute, ICallingInterceptor, ICalledInterceptor
    {
        private static readonly Lazy<IDistributedCache> _cacheLazy = new Lazy<IDistributedCache>(() => SingleServiceLocator.GetService<IDistributedCache>());

        private static IDistributedCache Cache { get => _cacheLazy.Value; }

        public CacheAttribute(string cacheKey)
        {
            Check.NotNullOrEmpty(cacheKey, nameof(cacheKey));
            CacheKey = cacheKey;
        }

        public CacheAttribute(string cacheKey, int cacheSeconds)
        {
            Check.NotNullOrEmpty(cacheKey, nameof(cacheKey));
            CacheKey = cacheKey;
            CacheSeconds = cacheSeconds;
        }

        public CacheAttribute(string cacheKey, int cacheSeconds, bool isSliding)
        {
            Check.NotNullOrEmpty(cacheKey, nameof(cacheKey));

            CacheKey = cacheKey;
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
            var result = Cache.Get<object>(CacheKey);
            if (result != null)
            {
                callingInterceptorContext.HasResult = true;
                callingInterceptorContext.Result = result;
            }
        }

        public void Called(CalledInterceptContext calledInterceptorContext)
        {
            if (!Cache.Exist(CacheKey))
            {
                var result = calledInterceptorContext.Result ?? calledInterceptorContext.ReturnValue;
                if (result != null)
                {
                    DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
                    if (IsSliding)
                    {
                        options.SetSlidingExpiration(TimeSpan.FromSeconds(CacheSeconds));
                    }
                    else
                    {
                        options.SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheSeconds));
                    }
                    Cache.Set(CacheKey, result, options);
                }
            }
        }
    }
}
