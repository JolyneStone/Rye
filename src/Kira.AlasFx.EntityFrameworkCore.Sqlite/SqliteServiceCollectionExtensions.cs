using Kira.AlasFx.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Kira.AlasFx.EntityFrameworkCore
{
    public static class SqliteServiceCollectionExtensions
    {
        /// <summary>
        /// 添加AlasFx框架对Sqlite数据库的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAlasFxSqlite(this IServiceCollection services)
        {
            services.TryAddSingleton<IDbContextOptionsBuilderUser, SqliteDbContextOptionsBuilderUser>();
            return services;
        }
    }
}
