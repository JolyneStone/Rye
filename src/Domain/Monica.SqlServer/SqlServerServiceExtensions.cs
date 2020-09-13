using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using System;

namespace Monica.SqlServer
{
    public static class SqlServerServiceExtensions
    {
        public static IServiceCollection AddSqlServerDbConnectionProvider<T>(this IServiceCollection services)
            where T : SqlServerConnectionProvider
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddScoped<SqlServerConnectionProvider, T>();
            return services;
        }

        public static IServiceCollection AddSqlServerDbConnectionProvider<T>(this IServiceCollection services, Func<IServiceProvider, T> providerFunc)
            where T : SqlServerConnectionProvider
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (providerFunc is null)
            {
                throw new ArgumentNullException(nameof(providerFunc));
            }
            services.TryAddScoped<SqlServerConnectionProvider>(providerFunc);
            return services;
        }

        public static IServiceCollection AddSqlServerDbConnectionProvider(this IServiceCollection services, Type providerType)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (providerType is null)
            {
                throw new ArgumentNullException(nameof(providerType));
            }

            services.TryAddScoped(typeof(SqlServerConnectionProvider), providerType);
            return services;
        }

        public static IServiceCollection AddSqlServerDbConnectionProvider(this IServiceCollection services, Func<IServiceProvider, object> providerFunc)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (providerFunc is null)
            {
                throw new ArgumentNullException(nameof(providerFunc));
            }

            services.TryAddScoped(typeof(SqlServerConnectionProvider), providerFunc);
            return services;
        }

        public static IServiceCollection AddSqlServerDbConnectionProvider<T>(this IServiceCollection services, ServiceLifetime serviceLifetime)
            where T : SqlServerConnectionProvider
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAdd(new ServiceDescriptor(typeof(SqlServerConnectionProvider), typeof(T), serviceLifetime));
            return services;
        }

        public static IServiceCollection AddSqlServerDbConnectionProvider<T>(this IServiceCollection services, Func<IServiceProvider, T> providerFunc, ServiceLifetime serviceLifetime)
            where T : SqlServerConnectionProvider
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (providerFunc is null)
            {
                throw new ArgumentNullException(nameof(providerFunc));
            }
            services.TryAdd(new ServiceDescriptor(typeof(SqlServerConnectionProvider), providerFunc, serviceLifetime));
            return services;
        }

        public static IServiceCollection AddSqlServerDbConnectionProvider(this IServiceCollection services, Type providerType, ServiceLifetime serviceLifetime)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (providerType is null)
            {
                throw new ArgumentNullException(nameof(providerType));
            }

            services.TryAdd(new ServiceDescriptor(typeof(SqlServerConnectionProvider), providerType, serviceLifetime));
            return services;
        }

        public static IServiceCollection AddSqlServerDbConnectionProvider(this IServiceCollection services, Func<IServiceProvider, object> providerFunc, ServiceLifetime serviceLifetime)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (providerFunc is null)
            {
                throw new ArgumentNullException(nameof(providerFunc));
            }

            services.TryAdd(new ServiceDescriptor(typeof(SqlServerConnectionProvider), providerFunc, serviceLifetime));
            return services;
        }
    }
}
