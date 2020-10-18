using Monica.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Monica.EntityFrameworkCore;

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
            services.AddMonicaDatabase();
            services.RemoveAll<IDbContextOptionsBuilderUser>();
            services.TryAddSingleton<IDbContextOptionsBuilderUser, SqliteDbContextOptionsBuilderUser>();
            return services;
        }

        /// <summary>
        /// 添加Monica框架对Sqlite数据库的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqliteEFCodeModule(this IServiceCollection services)
        {
            return services.AddModule<SqliteEFCoreModule>();
        }
    }
}
