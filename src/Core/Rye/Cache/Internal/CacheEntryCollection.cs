using Microsoft.Extensions.Caching.Distributed;

using Rye.Enums;
using Rye.Jwt;

using System;

namespace Rye.Cache.Internal
{
    public static class CacheEntryCollection
    {
        /// <summary>
        /// 获取权限缓存Key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static RyeCacheEntry GetPermissionEntry(string id, int expire = 15 * 60)
        {
            return new RyeCacheEntry
            {
                CacheKey = $"MC_Permission_{id}",
                Options = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expire)
                }
            };
        }

        /// <summary>
        /// 获取Token缓存Key
        /// </summary>
        /// <param name="clientType"></param>
        /// <param name="userId"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public static RyeCacheEntry GetTokenEntry(JwtTokenType tokenType, string clientType, string userId, int expire = 15 * 60)
        {
            return new RyeCacheEntry
            {
                CacheKey = $"MC_{tokenType.ToString()}:{userId}:{clientType}",
                Options = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expire)
                }
            };
        }

        /// <summary>
        /// 获取验证码缓存Key
        /// </summary>
        /// <param name="id"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public static RyeCacheEntry GetVerifyCodeEntry(string id, int expire = 5 * 60)
        {
            return new RyeCacheEntry
            {
                CacheKey = $"MC_VERIFY_CODE:{id}",
                Options = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expire)
                }
            };
        }
    }

    public sealed class RyeCacheEntry
    {
        public string CacheKey { get; set; }
        public DistributedCacheEntryOptions Options { get; set; }
    }
}
