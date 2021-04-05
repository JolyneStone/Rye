using Microsoft.Extensions.Caching.Memory;

using System;
using System.Threading.Tasks;

namespace Rye.Cache
{
    public static class MemoryCacheExtensions
    {
        /// <summary>
        /// 获取缓存，若不存在则创建，当 cacheSeconds == -1 时，表示永不过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="cacheSeconds"></param>
        /// <returns></returns>
        public static T GetOrCreate<T>(this IMemoryCache cache, object key, Func<T> func, int cacheSeconds = 60)
        {
            return cache.GetOrCreate(key, options =>
            {
                if (cacheSeconds != -1)
                    options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheSeconds);
                return func();
            });
        }

        /// <summary>
        /// 获取缓存，若不存在则创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static T GetOrCreate<T>(this IMemoryCache cache, object key, Func<T> func, MemoryCacheEntryOptions entry)
        {
            return cache.GetOrCreate(key, options =>
            {
                options.AbsoluteExpiration = entry.AbsoluteExpiration;
                options.AbsoluteExpirationRelativeToNow = entry.AbsoluteExpirationRelativeToNow;
                options.SlidingExpiration = entry.SlidingExpiration;
                return func();
            });
        }

        /// <summary>
        /// 获取缓存，若不存在则创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static T GetOrCreate<T>(this IMemoryCache cache, object key, Func<T> func, CacheOptionEntry entry)
        {
            return cache.GetOrCreate(key, options =>
            {
                options.AbsoluteExpiration = entry.AbsoluteExpiration;
                options.AbsoluteExpirationRelativeToNow = entry.AbsoluteExpirationRelativeToNow;
                options.SlidingExpiration = entry.SlidingExpiration;
                return func();
            });
        }

        /// <summary>
        /// 获取缓存，若不存在则创建，当 cacheSeconds == -1 时，表示永不过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="cacheSeconds"></param>
        /// <returns></returns>
        public static Task<T> GetOrCreateAsync<T>(this IMemoryCache cache, object key, Func<Task<T>> func, int cacheSeconds = 60)
        {
            return cache.GetOrCreateAsync(key, options =>
            {
                if (cacheSeconds != -1)
                    options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheSeconds);
                return func();
            });
        }

        /// <summary>
        /// 获取缓存，若不存在则创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static Task<T> GetOrCreateAsync<T>(this IMemoryCache cache, object key, Func<Task<T>> func, MemoryCacheEntryOptions entry)
        {
            return cache.GetOrCreateAsync(key, options =>
            {
                options.AbsoluteExpiration = entry.AbsoluteExpiration;
                options.AbsoluteExpirationRelativeToNow = entry.AbsoluteExpirationRelativeToNow;
                options.SlidingExpiration = entry.SlidingExpiration;
                return func();
            });
        }

        /// <summary>
        /// 获取缓存，若不存在则创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static Task<T> GetOrCreateAsync<T>(this IMemoryCache cache, object key, Func<Task<T>> func, CacheOptionEntry entry)
        {
            return cache.GetOrCreateAsync(key, options =>
            {
                options.AbsoluteExpiration = entry.AbsoluteExpiration;
                options.AbsoluteExpirationRelativeToNow = entry.AbsoluteExpirationRelativeToNow;
                options.SlidingExpiration = entry.SlidingExpiration;
                return func();
            });
        }
    }
}
