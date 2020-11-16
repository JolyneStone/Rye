using Monica.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Monica.EntityFrameworkCore;

namespace Monica
{
    public static class SqlServerServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Monica框架对Sql Server数据库的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMonicaSqlServer(this IServiceCollection services)
        {
            services.AddEFCoreDatabase();
            services.RemoveAll<IDbContextOptionsBuilderUser>();
            services.TryAddSingleton<IDbContextOptionsBuilderUser, SqlServerDbContextOptionsBuilderUser>();
            return services;
        }

        /// <summary>
        /// 添加Monica框架对Sql Server数据库的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlServerEFCodeModule(this IServiceCollection services)
        {
            return services.AddModule<SqlServerEFCoreModule>();
        }
    }
}
