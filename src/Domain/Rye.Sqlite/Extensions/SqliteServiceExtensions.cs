using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Rye.DataAccess;
using Rye.Entities.Abstractions;
using Rye.Sqlite.Service;

using System;

namespace Rye.Sqlite
{
    public static class SqliteServiceExtensions
    {
        /// <summary>
        /// 增加Sqlite模块
        /// </summary>
        /// <param name="services"></param>
        /// <param name="providerType"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqliteModule(this IServiceCollection services, Type providerType)
        {
            if (providerType is null)
            {
                throw new ArgumentNullException(nameof(providerType));
            }

            var module = new SqliteModule(providerType);
            return services.AddModule<SqliteModule>(module);
        }

        /// <summary>
        /// 增加Sqlite模块
        /// </summary>
        /// <param name="services"></param>
        /// <param name="providerFunc"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqliteModule(this IServiceCollection services, Func<IServiceProvider, object> providerFunc)
        {
            if (providerFunc is null)
            {
                throw new ArgumentNullException(nameof(providerFunc));
            }

            var module = new SqliteModule(providerFunc);
            return services.AddModule<SqliteModule>(module);
        }

        /// <summary>
        /// 增加Sqlite模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqliteModule<T>(this IServiceCollection services)
            where T : SqliteConnectionProvider
        {
            var module = new SqliteModule(typeof(T));
            return services.AddModule<SqliteModule>(module);
        }

        /// <summary>
        /// 增加Sqlite模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="providerFunc"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqliteModule<T>(this IServiceCollection services, Func<IServiceProvider, T> providerFunc)
        {
            if (providerFunc is null)
            {
                throw new ArgumentNullException(nameof(providerFunc));
            }

            object func(IServiceProvider serviceProvider) => (object)providerFunc(serviceProvider);
            var module = new SqliteModule(func);
            return services.AddModule<SqliteModule>(module);
        }

        public static IServiceCollection AddSqliteDbConnectionProvider<T>(this IServiceCollection services)
            where T: SqliteConnectionProvider
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddScoped<SqliteConnectionProvider, T>();
            AddSqliteInternalService(services);
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
            AddSqliteInternalService(services);
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
            AddSqliteInternalService(services);
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
            AddSqliteInternalService(services);
            return services;
        }

        public static IServiceCollection AddSqliteInternalService(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.RemoveAll<IPermissionService>();
            services.TryAddScoped<IPermissionService, SqlitePermissionService>();

            services.RemoveAll<ILangDictionaryService>();
            services.TryAddScoped<ILangDictionaryService, SqliteLangDictionaryService>();

            services.RemoveAll<IAppInfoService>();
            services.TryAddScoped<IAppInfoService, SqliteAppInfoService>();
            return services;
        }

        //public static IServiceCollection AddSqliteDbConnectionProvider<T>(this IServiceCollection services, ServiceLifetime serviceLifetime)
        //  where T : SqliteConnectionProvider
        //{
        //    if (services is null)
        //    {
        //        throw new ArgumentNullException(nameof(services));
        //    }

        //    services.TryAdd(new ServiceDescriptor(typeof(SqliteConnectionProvider), typeof(T), serviceLifetime));
        //    return services;
        //}

        //public static IServiceCollection AddSqliteDbConnectionProvider<T>(this IServiceCollection services, Func<IServiceProvider, T> providerFunc, ServiceLifetime serviceLifetime)
        //    where T : SqliteConnectionProvider
        //{
        //    if (services is null)
        //    {
        //        throw new ArgumentNullException(nameof(services));
        //    }

        //    if (providerFunc is null)
        //    {
        //        throw new ArgumentNullException(nameof(providerFunc));
        //    }
        //    services.TryAdd(new ServiceDescriptor(typeof(SqliteConnectionProvider), providerFunc, serviceLifetime));
        //    return services;
        //}

        //public static IServiceCollection AddSqliteDbConnectionProvider(this IServiceCollection services, Type providerType, ServiceLifetime serviceLifetime)
        //{
        //    if (services is null)
        //    {
        //        throw new ArgumentNullException(nameof(services));
        //    }

        //    if (providerType is null)
        //    {
        //        throw new ArgumentNullException(nameof(providerType));
        //    }

        //    services.TryAdd(new ServiceDescriptor(typeof(SqliteConnectionProvider), providerType, serviceLifetime));
        //    return services;
        //}

        //public static IServiceCollection AddSqliteDbConnectionProvider(this IServiceCollection services, Func<IServiceProvider, object> providerFunc, ServiceLifetime serviceLifetime)
        //{
        //    if (services is null)
        //    {
        //        throw new ArgumentNullException(nameof(services));
        //    }

        //    if (providerFunc is null)
        //    {
        //        throw new ArgumentNullException(nameof(providerFunc));
        //    }

        //    services.TryAdd(new ServiceDescriptor(typeof(SqliteConnectionProvider), providerFunc, serviceLifetime));
        //    return services;
        //}
    }
}
