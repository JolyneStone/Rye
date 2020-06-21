using Raven.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Raven.EntityFrameworkCore;

namespace Raven
{
    public static class SqliteServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Raven框架对Sqlite数据库的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRavenSqlite(this IServiceCollection services)
        {
            services.AddRavenDatabase();
            services.RemoveAll<IDbContextOptionsBuilderUser>();
            services.TryAddSingleton<IDbContextOptionsBuilderUser, SqliteDbContextOptionsBuilderUser>();
            return services;
        }
    }
}
