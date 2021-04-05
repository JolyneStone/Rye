using Dapper;

using Demo.Core.Common;

using Rye.Cache.Store;
using Rye.MySql;

using System.Threading.Tasks;

namespace Demo.DataAccess
{
    public partial class DaoAppInfo
    {
        private readonly ICacheStore _store;
        public DaoAppInfo(MySqlConnectionProvider provider, ICacheStore store)
        {
            ConnectionProvider = provider;
            _store = store;
        }

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

        public AppInfo GetModelWithCache(string appKey)
        {
            var enrty = CacheEntrys.AppInfoByKey(appKey);
            var appInfo = _store.Get<AppInfo>(enrty);
            if (appInfo != null)
                return appInfo;

            appInfo = GetModel(appKey);
            if (appInfo != null)
                _store.Set(enrty, appInfo);
            return appInfo;
        }

        public AppInfo GetModelWithCache(int appId)
        {
            var enrty = CacheEntrys.AppInfoById(appId);
            var appInfo = _store.Get<AppInfo>(enrty);
            if (appInfo != null)
                return appInfo;

            appInfo = GetModel(appId);
            if (appInfo != null)
                _store.Set(enrty, appInfo);
            return appInfo;
        }

        public async Task<AppInfo> GetModelWithCacheAsync(string appKey)
        {
            var enrty = CacheEntrys.AppInfoByKey(appKey);
            var appInfo = await _store.GetAsync<AppInfo>(enrty);
            if (appInfo != null)
                return appInfo;

            appInfo = GetModel(appKey);
            if (appInfo != null)
                await _store.SetAsync(enrty, appInfo);
            return appInfo;
        }

        public async Task<AppInfo> GetModelWithCacheAsync(int appId)
        {
            var enrty = CacheEntrys.AppInfoById(appId);
            var appInfo = await _store.GetAsync<AppInfo>(enrty);
            if (appInfo != null)
                return appInfo;

            appInfo = GetModel(appId);
            if (appInfo != null)
                await _store.SetAsync(enrty, appInfo);
            return appInfo;
        }
    }
}
