using System.Threading.Tasks;

namespace Demo.DataAccess
{
    public partial interface IAppInfo
    {
        AppInfo GetModel(string appKey);

        AppInfo GetModelWithCache(string appKey);

        AppInfo GetModelWithCache(int appId);

        Task<AppInfo> GetModelWithCacheAsync(string appKey);

        Task<AppInfo> GetModelWithCacheAsync(int appId);
    }
}
