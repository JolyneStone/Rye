using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

using System;

namespace Rye.Cache
{
    public static class CacheOptionEntryExtensions
    {
        /// <summary>
        /// 将 CacheOptionEntry 对象实例，转化为 DistributedCacheEntryOptions
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static DistributedCacheEntryOptions ConvertToDistributedCacheEntry(this CacheOptionEntry entry)
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = entry.AbsoluteExpiration,
                AbsoluteExpirationRelativeToNow = entry.AbsoluteExpirationRelativeToNow,
                SlidingExpiration = entry.SlidingExpiration
            };
        }

        /// <summary>
        /// 将 CacheOptionEntry 对象实例，转化为 MemoryCacheEntryOptions
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static MemoryCacheEntryOptions ConvertToMemoryCacheEntryOptions(this CacheOptionEntry entry)
        {
            return new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = entry.AbsoluteExpiration,
                AbsoluteExpirationRelativeToNow = entry.AbsoluteExpirationRelativeToNow,
                SlidingExpiration = entry.SlidingExpiration
            };
        }

        /// <summary>
        /// 将 CacheOptionEntry 对象实例，转化为相对与当前时间的 TimeSpan
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">当 CacheOptionEntry 对象的 
        /// <see cref="CacheOptionEntry.AbsoluteExpirationRelativeToNow"/>属性和<see cref="CacheOptionEntry.AbsoluteExpiration"/>都为null时</exception>
        public static TimeSpan ConvertToTimeSpanRelativeToNow(this CacheOptionEntry entry)
        {
            if (entry.AbsoluteExpirationRelativeToNow.HasValue)
                return entry.AbsoluteExpirationRelativeToNow.Value;
            if (entry.AbsoluteExpiration.HasValue)
                return entry.AbsoluteExpiration.Value - DateTimeOffset.UtcNow;
            if (entry.SlidingExpiration.HasValue)
                throw new NotSupportedException($"Unable to convert {nameof(CacheOptionEntry.SlidingExpiration)} to TimeSpan for relative to now. This operation is not supported.");

            throw new NotSupportedException($"Unable to convert {nameof(CacheOptionEntry)} to TimeSpan for relative to now. This operation is not supported.");
        }

        /// <summary>
        /// 设置以对当前时间为基础时间的绝对过期时间
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="relative"></param>
        /// <returns></returns>
		public static CacheOptionEntry SetAbsoluteExpiration(this CacheOptionEntry entry, TimeSpan relative)
        {
            entry.AbsoluteExpirationRelativeToNow = new TimeSpan?(relative);
            return entry;
        }

        /// <summary>
        /// 设置绝对过期时间
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="absolute"></param>
        /// <returns></returns>
        public static CacheOptionEntry SetAbsoluteExpiration(this CacheOptionEntry entry, DateTimeOffset absolute)
        {
            entry.AbsoluteExpiration = new DateTimeOffset?(absolute);
            return entry;
        }

        /// <summary>
        /// 设置滑动过期时间
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static CacheOptionEntry SetSlidingExpiration(this CacheOptionEntry entry, TimeSpan offset)
        {
            entry.SlidingExpiration = new TimeSpan?(offset);
            return entry;
        }
    }
}
