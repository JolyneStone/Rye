using Dapper;

using Rye.Cache;
using Rye.Cache.Internal;
using Rye.Entities.Abstractions;
using Rye.Enums;

namespace Rye.Sqlite.Service
{
    public class SqliteAppInfoService: IAppInfoService
    {
        private readonly SqliteConnectionProvider _connectionProvider;

        public SqliteAppInfoService(SqliteConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public string GetAppSecret(string appKey)
        {
            var entry = MemoryCacheEntryCollection.GetAppSecretEntry(appKey);
            var appSecret = MemoryCacheManager.Get<string>(entry.CacheKey);
            if (!appSecret.IsNullOrEmpty())
                return appSecret;

            var sql = "select appSecret from appInfo where appKey = @appKey and status = @status limit 1";
            var parameter = new DynamicParameters();
            parameter.Add("@appKey", appKey);
            parameter.Add("@status", (int)EntityStatus.Enabled);
            using (var conn = _connectionProvider.GetReadOnlyConnection())
            {
                appSecret = conn.QueryFirstOrDefault<string>(sql, param: parameter);
            }

            if (!appSecret.IsNullOrEmpty())
                MemoryCacheManager.Set(entry.CacheKey, appSecret, entry.Expire);
            return appSecret;
        }
    }
}
