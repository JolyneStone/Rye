using Dapper;

using Demo.Common;

using Rye.Cache.Redis;
using Rye.Cache.Store;
using Rye.MySql;

using System.Threading.Tasks;

namespace Demo.DataAccess
{
    public partial class DaoAppInfo
    {
        public AppInfo GetModel(string appKey)
        {
            var sql = $"select {GetColumns()} from AppInfo where appKey = @appKey";
            var param = new DynamicParameters();
            param.Add("@appKey", appKey);

            using (var conn = ConnectionProvider.GetReadOnlyConnection())
            {
                return conn.Connection.QueryFirstOrDefault<AppInfo>(sql, param: param);
            }
        }

        public async Task<AppInfo> GetModelAsync(string appKey)
        {
            var sql = $"select {GetColumns()} from AppInfo where appKey = @appKey";
            var param = new DynamicParameters();
            param.Add("@appKey", appKey);

            using (var conn = ConnectionProvider.GetReadOnlyConnection())
            {
                return await conn.Connection.QueryFirstOrDefaultAsync<AppInfo>(sql, param: param);
            }
        }

        public AppInfo GetModelWithCache(string appKey)
        {
            //var enrty = CacheEntrys.AppInfoByKey(appKey);
            //var appInfo = _store.Get<AppInfo>(enrty);
            //if (appInfo != null)
            //    return appInfo;

            //appInfo = GetModel(appKey);
            //if (appInfo != null)
            //    _store.Set(enrty, appInfo);
            //return appInfo;
            return GetModel(appKey);
        }

        public AppInfo GetModelWithCache(int appId)
        {
            //var enrty = CacheEntrys.AppInfoById(appId);
            //var appInfo = _store.Get<AppInfo>(enrty);
            //if (appInfo != null)
            //    return appInfo;

            //appInfo = GetModel(appId);
            //if (appInfo != null)
            //    _store.Set(enrty, appInfo);
            //return appInfo;
            return GetModel(appId);
        }

        public async Task<AppInfo> GetModelWithCacheAsync(string appKey)
        {
            return await GetModelAsync(appKey);
        }

        public async Task<AppInfo> GetModelWithCacheAsync(int appId)
        {
            return await GetModelAsync(appId);
        }
    }
}
