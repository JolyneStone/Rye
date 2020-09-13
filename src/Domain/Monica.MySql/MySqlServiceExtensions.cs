using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Monica.MySql
{
    public static class MySqlServiceExtensions
    {
        public static IServiceCollection AddMySqlDbConnectionProvider<T>(this IServiceCollection services)
            where T: MySqlConnectionProvider
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddScoped<MySqlConnectionProvider, T>();
            return services;
        }

        public static IServiceCollection AddMySqlDbConnectionProvider<T>(this IServiceCollection services, Func<IServiceProvider, T> providerFunc)
            where T : MySqlConnectionProvider
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (providerFunc is null)
            {
                throw new ArgumentNullException(nameof(providerFunc));
            }
            services.TryAddScoped<MySqlConnectionProvider>(providerFunc);
            return services;
        }

        public static IServiceCollection AddMySqlDbConnectionProvider(this IServiceCollection services, Type providerType)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (providerType is null)
            {
                throw new ArgumentNullException(nameof(providerType));
            }

            services.TryAddScoped(typeof(MySqlConnectionProvider), providerType);
            return services;
        }

        public static IServiceCollection AddMySqlDbConnectionProvider(this IServiceCollection services, Func<IServiceProvider, object> providerFunc)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (providerFunc is null)
            {
                throw new ArgumentNullException(nameof(providerFunc));
            }

            services.TryAddScoped(typeof(MySqlConnectionProvider), providerFunc);
            return services;
        }

        public static IServiceCollection AddMySqlDbConnectionProvider<T>(this IServiceCollection services, ServiceLifetime serviceLifetime)
         where T : MySqlConnectionProvider
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAdd(new ServiceDescriptor(typeof(MySqlConnectionProvider), typeof(T), serviceLifetime));
            return services;
        }

        public static IServiceCollection AddMySqlDbConnectionProvider<T>(this IServiceCollection services, Func<IServiceProvider, T> providerFunc, ServiceLifetime serviceLifetime)
            where T : MySqlConnectionProvider
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (providerFunc is null)
            {
                throw new ArgumentNullException(nameof(providerFunc));
            }
            services.TryAdd(new ServiceDescriptor(typeof(MySqlConnectionProvider), providerFunc, serviceLifetime));
            return services;
        }

        public static IServiceCollection AddMySqlDbConnectionProvider(this IServiceCollection services, Type providerType, ServiceLifetime serviceLifetime)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (providerType is null)
            {
                throw new ArgumentNullException(nameof(providerType));
            }

            services.TryAdd(new ServiceDescriptor(typeof(MySqlConnectionProvider), providerType, serviceLifetime));
            return services;
        }

        public static IServiceCollection AddMySqlDbConnectionProvider(this IServiceCollection services, Func<IServiceProvider, object> providerFunc, ServiceLifetime serviceLifetime)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (providerFunc is null)
            {
                throw new ArgumentNullException(nameof(providerFunc));
            }

            services.TryAdd(new ServiceDescriptor(typeof(MySqlConnectionProvider), providerFunc, serviceLifetime));
            return services;
        }
    }
}
