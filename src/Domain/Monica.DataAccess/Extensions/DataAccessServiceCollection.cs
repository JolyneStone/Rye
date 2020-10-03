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
    }
}
