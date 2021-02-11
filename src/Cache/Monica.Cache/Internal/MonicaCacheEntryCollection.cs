using Microsoft.Extensions.Caching.Distributed;
using System;

namespace Monica.Cache.Internal
{
    public static class MonicaCacheEntryCollection
    {
        /// <summary>
        /// 获取权限缓存
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MonicaCacheEntry GetPermissionEntry(string id)
        {
            return new MonicaCacheEntry
            {
                CacheKey = $"M_Permission_{id}",
                Options = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
                }
            };
        }
    }

    public sealed class MonicaCacheEntry
    {
        public string CacheKey { get; set; }
        public DistributedCacheEntryOptions Options { get; set; }
    }
}
