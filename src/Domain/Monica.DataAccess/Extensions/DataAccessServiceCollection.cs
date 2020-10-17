using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using Monica.DataAccess.Options;

using System;

namespace Monica
{
    public static class DataAccessServiceCollection
    {
        public static IServiceCollection AddDbConnections(this IServiceCollection serviceCollection, Action<DbConnectionMapOptions> action = null)
        {
            serviceCollection.RemoveAll<IOptions<DbConnectionMapOptions>>();
            if (action == null)
            {
                var congiration = serviceCollection.GetSingletonInstance<IConfiguration>();
                action = options =>
                {
                    congiration.GetSection("Framework:DbConnections").Bind(options);
                };
            }
            serviceCollection.Configure<DbConnectionMapOptions>(action);
            //serviceCollection.TryAddSingleton<IConfigureOptions<DbConnectionMapOptions>, DbConnectionsMapOptionsSetup>();
            return serviceCollection;
        }

        /// <summary>
        /// 添加Monica框架对数据库访问层的支持
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataAccessModule(this IServiceCollection services, Action<DbConnectionMapOptions> action = null)
        {
            services.AddDbConnections(action);
            return services;
        }

        /// <summary>
        /// 添加Monica框架对数据库访问层的支持
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationSection"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataAccessModule(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            services.AddDbConnections(options => configurationSection.Bind(options));
            return services;
        }
    }
}
