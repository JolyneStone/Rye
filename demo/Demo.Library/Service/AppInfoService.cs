using Dapper;

using Demo.Library.Abstraction;

using Rye;
using Rye.Cache;
using Rye.Cache.Store;
using Rye.Enums;
using Rye.MySql;
using Rye.Web;
using Rye.Web.Abstraction;

using System.Threading.Tasks;

namespace Demo.Library.Service
{
    public class AppInfoService : IAppInfoService
    {
        private readonly MySqlConnectionProvider _connectionProvider;
        private readonly ICacheStore _store;

        public AppInfoService(MySqlConnectionProvider connectionProvider, ICacheStore store)
        {
            _connectionProvider = connectionProvider;
            _store = store;
        }

        public string GetAppKey()
        {
            return WebApp.HttpContext?.Request.GetString("appKey");
        }

        public string GetAppSecret(string appKey)
        {
            if (appKey.IsNullOrEmpty())
                return default;

            var entry = CacheEntryCollection.GetAppSecretEntry(appKey);
            var appSecret = _store.Get<string>(entry);
            if (!appSecret.IsNullOrEmpty())
                return appSecret;

            var sql = "select `appSecret` from `appInfo` where appKey = @appKey and status = @status limit 1";
            var parameter = new DynamicParameters();
            parameter.Add("@appKey", appKey);
            parameter.Add("@status", (int)EntityStatus.Enabled);
            using (var conn = _connectionProvider.GetReadOnlyConnection())
            {
                appSecret = conn.Connection.QueryFirstOrDefault<string>(sql, param: parameter);
            }

            if (!appSecret.IsNullOrEmpty())
                _store.Set(entry, appSecret);
            return appSecret;
        }
    }
}
