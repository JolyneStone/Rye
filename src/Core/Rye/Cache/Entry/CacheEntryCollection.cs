using Rye.Jwt;

using System;

namespace Rye.Cache
{
    public static class CacheEntryCollection
    {
        /// <summary>
        /// 获取权限缓存Key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CacheOptionEntry GetPermissionEntry(string id, int expire = 15 * 60)
        {
            return new CacheOptionEntry
            {
                Key = $"Rye_Permission_{id}",
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expire)
            };
        }

        /// <summary>
        /// 获取Token缓存Key
        /// </summary>
        /// <param name="clientType"></param>
        /// <param name="userId"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public static CacheOptionEntry GetTokenEntry(JwtTokenType tokenType, string clientType, string userId, int expire = 15 * 60)
        {
            return new CacheOptionEntry
            {
                Key = $"Rye_{tokenType.ToString()}:{userId}:{clientType}",
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expire)
            };
        }

        /// <summary>
        /// 获取验证码缓存Key
        /// </summary>
        /// <param name="id"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public static CacheOptionEntry GetVerifyCodeEntry(string id, int expire = 5 * 60)
        {
            return new CacheOptionEntry
            {
                Key = $"Rye_Verify_Code:{id}",
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expire)
            };
        }

        /// <summary>
        /// 获取AppInfo Sign 缓存Key
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public static CacheOptionEntry GetAppInfoSignEntry(string appId, int expire = 24 * 60 * 60)
        {
            return new CacheOptionEntry
            {
                Key = $"Rye_AppInfo:Sign:{appId}",
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expire)
            };
        }

        /// <summary>
        /// 获取AppInfo AppSecret 缓存Key
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public static CacheOptionEntry GetAppSecretEntry(string appKey, int expire = 24 * 60 * 60)
        {
            return new CacheOptionEntry
            {
                Key = $"Rye_AppInfo:AppSecret:{appKey}",
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expire)
            };
        }
    }
}
