using Monica.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Monica.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Monica
{
    public static class SqliteServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Monica框架对Sqlite数据库的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMonicaSqlite(this IServiceCollection services)
        {
            services.AddEFCoreDatabase();
            services.RemoveAll<IDbContextOptionsBuilderUser>();
            services.TryAddSingleton<IDbContextOptionsBuilderUser, SqliteDbContextOptionsBuilderUser>();
            return services;
        }

        /// <summary>
        /// 添加Monica框架对Sqlite数据库的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqliteEFCodeModule(this IServiceCollection services, Action<MonicaDbContextOptionsBuilder> action)
        {
            var module = new SqliteEFCoreModule(action);
            return services.AddModule<SqliteEFCoreModule>(module);
        }
    }
}
