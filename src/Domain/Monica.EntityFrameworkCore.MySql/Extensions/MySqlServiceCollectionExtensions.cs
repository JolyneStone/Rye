using Monica.EntityFrameworkCore.MySql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Monica.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Monica
{
    public static class MySqlServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Monica框架对MySql数据库的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMonicaMySql(this IServiceCollection services)
        {
            services.AddEFCoreDatabase();
            services.RemoveAll<IDbContextOptionsBuilderUser>();
            services.TryAddSingleton<IDbContextOptionsBuilderUser, MySqlDbContextOptionsBuilderUser>();
            return services;
        }

        /// <summary>
        /// 添加Monica框架对MySql数据库的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMySqlEFCodeModule(this IServiceCollection services, Action<MonicaDbContextOptionsBuilder> action)
        {
            var module = new MySqlEFCoreModule(action);
            return services.AddModule<MySqlEFCoreModule>(module);
        }
    }
}
