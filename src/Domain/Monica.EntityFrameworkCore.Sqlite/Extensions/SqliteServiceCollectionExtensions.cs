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
        public static IServiceCollection AddSqliteEFCodeModule<TContext>(this IServiceCollection services, string dbName = null, Action<DbContextOptionsBuilder<TContext>> action = null)
            where TContext : DbContext
        {
            var module = new SqliteEFCoreModule<TContext>(dbName, action);
            return services.AddModule<SqliteEFCoreModule<TContext>>(module);
        }
    }
}
