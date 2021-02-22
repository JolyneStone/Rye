using Rye.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rye.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Rye
{
    public static class SqlServerServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Rye框架对Sql Server数据库的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRyeSqlServer(this IServiceCollection services)
        {
            services.AddEFCoreDatabase();
            services.RemoveAll<IDbContextOptionsBuilderUser>();
            services.TryAddSingleton<IDbContextOptionsBuilderUser, SqlServerDbContextOptionsBuilderUser>();
            return services;
        }

        /// <summary>
        /// 添加Rye框架对Sql Server数据库的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlServerEFCodeModule(this IServiceCollection services, Action<RyeDbContextOptionsBuilder> action)
        {
            var module = new SqlServerEFCoreModule(action);
            return services.AddModule<SqlServerEFCoreModule>(module);
        }
    }
}
