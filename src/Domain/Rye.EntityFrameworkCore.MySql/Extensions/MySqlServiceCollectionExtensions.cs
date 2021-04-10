using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Rye.EntityFrameworkCore;
using Rye.EntityFrameworkCore.MySql;

using System;

namespace Rye
{
    public static class MySqlServiceCollectionExtensions
    {
        /// <summary>
        /// 添加EF Core框架对MySql数据库的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMySqlEFCore(this IServiceCollection services)
        {
            services.AddEFCoreDatabase();
            services.RemoveAll<IDbContextOptionsBuilderUser>();
            services.TryAddSingleton<IDbContextOptionsBuilderUser, MySqlDbContextOptionsBuilderUser>();
            return services;
        }

        /// <summary>
        /// 添加EF Core框架对MySql数据库的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMySqlEFCoreModule(this IServiceCollection services, Action<RyeDbContextOptionsBuilder> action)
        {
            var module = new MySqlEFCoreModule(action);
            return services.AddModule<MySqlEFCoreModule>(module);
        }
    }
}
