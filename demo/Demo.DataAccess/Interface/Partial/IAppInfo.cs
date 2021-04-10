using Rye.Cache.Redis;

using System.Threading.Tasks;

namespace Demo.DataAccess
{
    public partial interface IAppInfo
    {
        AppInfo GetModel(string appKey);
        Task<AppInfo> GetModelAsync(string appKey);

        [RedisCache(cacheKey: "AppInfoByKey", cacheSeconds: 60 * 5)]
        AppInfo GetModelWithCache(string appKey);

        [RedisCache(cacheKey: "AppInfoByKey", cacheSeconds: 60 * 5)]
        Task<AppInfo> GetModelWithCacheAsync(string appKey);

        [RedisCache(cacheKey: "AppInfoById", cacheSeconds: 60 * 5)]
        AppInfo GetModelWithCache(int appId);

        [RedisCache(cacheKey: "AppInfoById", cacheSeconds: 60 * 5)]
        Task<AppInfo> GetModelWithCacheAsync(int appId);
    }
}
