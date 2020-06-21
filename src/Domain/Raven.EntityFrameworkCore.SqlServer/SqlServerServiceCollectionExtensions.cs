using Raven.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Raven.EntityFrameworkCore;

namespace Raven
{
    public static class SqlServerServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Raven框架对Sql Server数据库的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRavenSqlServer(this IServiceCollection services)
        {
            services.AddRavenDatabase();
            services.RemoveAll<IDbContextOptionsBuilderUser>();
            services.TryAddSingleton<IDbContextOptionsBuilderUser, SqlServerDbContextOptionsBuilderUser>();
            return services;
        }
    }
}
