using Microsoft.Extensions.Caching.Distributed;

using Monica.Enums;
using Monica.Jwt;

using System;

namespace Monica.Cache.Internal
{
    public static class CacheEntryCollection
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
                CacheKey = $"MC_Permission_{id}",
                Options = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
                }
            };
        }

        /// <summary>
        /// 获取Token缓存
        /// </summary>
        /// <param name="clientType"></param>
        /// <param name="userId"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public static MonicaCacheEntry GetTokenEntry(JwtTokenType tokenType, string clientType, string userId, int expire)
        {
            return new MonicaCacheEntry
            {
                CacheKey = $"MC_{tokenType.ToString()}:{userId}:{clientType}",
                Options = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expire)
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
