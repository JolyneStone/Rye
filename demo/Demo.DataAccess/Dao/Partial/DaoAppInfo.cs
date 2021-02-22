using Dapper;

using Demo.Core.Common;

using Rye.Cache;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            using(var conn = ConnectionProvider.GetReadOnlyConnection())
            {
                return conn.QueryFirstOrDefault<AppInfo>(sql, param: param);
            }
        }

        public AppInfo GetModelWithCache(string appKey)
        {
            var key = string.Format(MemoryCacheKeys.AppInfoByKey, appKey);
            var appInfo = MemoryCacheManager.Get<AppInfo>(key);
            if (appInfo != null)
                return appInfo;

            appInfo = GetModel(appKey);
            if (appInfo != null)
                MemoryCacheManager.Set(key, appInfo, MemoryCacheKeys.AppInfoByKey_TimeOut);
            return appInfo;
        }

        public AppInfo GetModelWithCache(int appId)
        {
            var key = string.Format(MemoryCacheKeys.AppInfoById, appId);
            var appInfo = MemoryCacheManager.Get<AppInfo>(key);
            if (appInfo != null)
                return appInfo;

            appInfo = GetModel(appId);
            if (appInfo != null)
                MemoryCacheManager.Set(key, appInfo, MemoryCacheKeys.AppInfoById_TimeOut);
            return appInfo;
        }
    }
}
