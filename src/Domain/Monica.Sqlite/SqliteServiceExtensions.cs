using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Monica.Sqlite
{
    public static class SqliteServiceExtensions
    {
        public static IServiceCollection AddSqliteDbConnectionProvider<T>(this IServiceCollection services)
            where T: SqliteConnectionProvider
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddScoped<SqliteConnectionProvider, T>();
            return services;
        }

        public static IServiceCollection AddSqliteDbConnectionProvider<T>(this IServiceCollection services, Func<IServiceProvider, T> providerFunc)
            where T : SqliteConnectionProvider
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (providerFunc is null)
            {
                throw new ArgumentNullException(nameof(providerFunc));
            }
            services.TryAddScoped<SqliteConnectionProvider>(providerFunc);
            return services;
        }

        public static IServiceCollection AddSqliteDbConnectionProvider(this IServiceCollection services, Type providerType)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (providerType is null)
            {
                throw new ArgumentNullException(nameof(providerType));
            }

            services.TryAddScoped(typeof(SqliteConnectionProvider), providerType);
            return services;
        }

        public static IServiceCollection AddSqliteDbConnectionProvider(this IServiceCollection services, Func<IServiceProvider, object> providerFunc)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (providerFunc is null)
            {
                throw new ArgumentNullException(nameof(providerFunc));
            }

            services.TryAddScoped(typeof(SqliteConnectionProvider), providerFunc);
            return services;
        }

        public static IServiceCollection AddSqliteDbConnectionProvider<T>(this IServiceCollection services, ServiceLifetime serviceLifetime)
          where T : SqliteConnectionProvider
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAdd(new ServiceDescriptor(typeof(SqliteConnectionProvider), typeof(T), serviceLifetime));
            return services;
        }

        public static IServiceCollection AddSqliteDbConnectionProvider<T>(this IServiceCollection services, Func<IServiceProvider, T> providerFunc, ServiceLifetime serviceLifetime)
            where T : SqliteConnectionProvider
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (providerFunc is null)
            {
                throw new ArgumentNullException(nameof(providerFunc));
            }
            services.TryAdd(new ServiceDescriptor(typeof(SqliteConnectionProvider), providerFunc, serviceLifetime));
            return services;
        }

        public static IServiceCollection AddSqliteDbConnectionProvider(this IServiceCollection services, Type providerType, ServiceLifetime serviceLifetime)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (providerType is null)
            {
                throw new ArgumentNullException(nameof(providerType));
            }

            services.TryAdd(new ServiceDescriptor(typeof(SqliteConnectionProvider), providerType, serviceLifetime));
            return services;
        }

        public static IServiceCollection AddSqliteDbConnectionProvider(this IServiceCollection services, Func<IServiceProvider, object> providerFunc, ServiceLifetime serviceLifetime)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (providerFunc is null)
            {
                throw new ArgumentNullException(nameof(providerFunc));
            }

            services.TryAdd(new ServiceDescriptor(typeof(SqliteConnectionProvider), providerFunc, serviceLifetime));
            return services;
        }
    }
}
