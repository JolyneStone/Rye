using Monica.DataAccess;
using Monica.EntityFrameworkCore.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Monica.EntityFrameworkCore;

namespace Monica
{
    public static class EFCoreServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Monica框架对数据库的支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddEFCoreDatabase(this IServiceCollection services)
        {
            services.RemoveAll<IDbProvider>();
            services.RemoveAll<IUnitOfWorkFactory>();
            services.RemoveAll<IRepositoryFactory>();
            services.TryAddScoped<IDbProvider, DbProvider>();
            services.TryAddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
            services.TryAddSingleton<IRepositoryFactory, RepositoryFactory>();
            return services;
        }

        ///// <summary>
        ///// 添加Monica框架对EF Core的支持
        ///// </summary>
        ///// <param name="services"></param>
        ///// <param name="action"></param>
        ///// <returns></returns>
        //public static IServiceCollection AddEFCoreModule(this IServiceCollection services, Action<DbContextOptionsBuilderOptions> action)
        //{
        //    var module = new EFCoreModule(action);
        //    services.AddModule<EFCoreModule>(module);
        //    return services;
        //}

        ///// <summary>
        ///// 添加Monica框架对EF Core的支持
        ///// </summary>
        ///// <param name="services"></param>
        ///// <param name="dbName"></param>
        ///// <param name="action"></param>
        ///// <returns></returns>
        //public static IServiceCollection AddEFCoreModule(this IServiceCollection services, string dbName = null, Action<DbContextOptionsBuilder> action = null)
        //{
        //    var builder = new DbContextOptionsBuilder();
        //    action?.Invoke(builder);
        //    var module = new EFCoreModule(options =>
        //    {
        //        options.Builder = builder;
        //        options.DbName = dbName;
        //    });
        //    services.AddModule<EFCoreModule>(module);
        //    return services;
        //}

        ///// <summary>
        ///// 添加DbContextOptionsBuilderOptions配置选项
        ///// </summary>
        ///// <param name="services"></param>
        ///// <param name="dbName"></param>
        ///// <param name="builderAction"></param>
        ///// <returns></returns>
        //public static IServiceCollection AddDbBuilderOptions(this IServiceCollection services, string dbName = null, Action<DbContextOptionsBuilder> builderAction = null)
        //{
        //    var builder = new DbContextOptionsBuilder();
        //    builderAction?.Invoke(builder);
        //    services.TryAddSingleton<DbContextOptionsBuilderOptions>(new DbContextOptionsBuilderOptions(builder, dbName));
        //    return services;
        //}

        /// <summary>
        /// 添加DbContextOptionsBuilderOptions配置选项
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dbName"></param>
        /// <param name="builderAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddDbBuilderOptions<TContext>(this IServiceCollection services, string dbName = null, Action<DbContextOptionsBuilder<TContext>> builderAction = null)
            where TContext : DbContext, IDbContext
        {
            var builder = new DbContextOptionsBuilder<TContext>();
            builderAction?.Invoke(builder);
            services.TryAddSingleton<DbContextOptionsBuilderOptions<TContext>>(new DbContextOptionsBuilderOptions<TContext>(builder, dbName));
            services.TryAddScoped<IUnitOfWork>(service => service.GetRequiredService<IDbProvider>().GetUnitOfWork<TContext>());
            return services;
        }

        /// <summary>
        /// 添加DbContextOptionsBuilderOptions配置选项
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddDbBuillderOptions<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilderOptions<TContext>> action)
               where TContext : DbContext, IDbContext
        {
            var options = new DbContextOptionsBuilderOptions<TContext>();
            action?.Invoke(options);
            services.TryAddSingleton<DbContextOptionsBuilderOptions<TContext>>(options);
            services.TryAddScoped<IUnitOfWork>(service => service.GetRequiredService<IDbProvider>().GetUnitOfWork<TContext>());
            return services;
        }

        /// <summary>
        /// 添加DbContext配置选项
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddDbBuillderOptions(this IServiceCollection services, Action<MonicaDbContextOptionsBuilder> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new MonicaDbContextOptionsBuilder();
            action?.Invoke(builder);
            var dbContextOptions = builder.Options;
            if (dbContextOptions == null || dbContextOptions.Count <= 0)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var method = typeof(EFCoreServiceCollectionExtensions).GetMethod("AddDbContextOptionsBuilderOptionsPrivate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            foreach (var pair in dbContextOptions)
            {
                method.MakeGenericMethod(pair.Key).Invoke(null, new object[] { services, pair.Value });
            }
            return services;
        }

        private static void AddDbContextOptionsBuilderOptionsPrivate<TContext>(IServiceCollection serviceCollection, object options)
            where TContext : DbContext, IDbContext
        {
            serviceCollection.TryAddSingleton<DbContextOptionsBuilderOptions<TContext>>((DbContextOptionsBuilderOptions<TContext>)options);
            serviceCollection.TryAddScoped<IUnitOfWork>(service => service.GetRequiredService<IDbProvider>().GetUnitOfWork<TContext>());
        }
    }
}
