using Demo.Common;

using Rye;
using Rye.Cache.Redis;

using System.Threading.Tasks;

namespace Demo.DataAccess
{
    public partial interface IAppInfo
    {
        AppInfo GetModel(string appKey);
        Task<AppInfo> GetModelAsync(string appKey);

        [Cache(CacheScheme.Redis, cacheKey: "AppInfoByKey", cacheSeconds: CacheStrategy.FIVE_MINUTES)]
        AppInfo GetModelWithCache(string appKey);

        [Cache(CacheScheme.Redis, cacheKey: "AppInfoByKey", cacheSeconds: CacheStrategy.FIVE_MINUTES)]
        Task<AppInfo> GetModelWithCacheAsync(string appKey);

        [Cache(CacheScheme.Redis, cacheKey: "AppInfoById", cacheSeconds: CacheStrategy.FIVE_MINUTES)]
        AppInfo GetModelWithCache(int appId);

        [Cache(CacheScheme.Redis, cacheKey: "AppInfoById", cacheSeconds: CacheStrategy.FIVE_MINUTES)]
        Task<AppInfo> GetModelWithCacheAsync(int appId);
    }
}
