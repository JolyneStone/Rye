using Raven.EntityFrameworkCore.MySql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Raven.EntityFrameworkCore;

namespace Raven
{
    public static class MySqlServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Raven框架对MySql数据库的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRavenMySql(this IServiceCollection services)
        {
            services.AddRavenDatabase();
            services.RemoveAll<IDbContextOptionsBuilderUser>();
            services.TryAddSingleton<IDbContextOptionsBuilderUser, MySqlDbContextOptionsBuilderUser>();
            return services;
        }
    }
}
