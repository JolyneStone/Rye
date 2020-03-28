using KiraNet.AlasFx.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KiraNet.AlasFx.EntityFrameworkCore
{
    public static class SqlServerServiceCollectionExtensions
    {
        /// <summary>
        /// 添加AlasFx框架对Sql Server数据库的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAlasFxSqlServer(this IServiceCollection services)
        {
            services.AddAlasFxDatabase();
            services.RemoveAll<IDbContextOptionsBuilderUser>();
            services.TryAddSingleton<IDbContextOptionsBuilderUser, SqlServerDbContextOptionsBuilderUser>();
            return services;
        }
    }
}
