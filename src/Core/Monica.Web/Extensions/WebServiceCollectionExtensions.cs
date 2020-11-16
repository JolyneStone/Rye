using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Monica.Module;
using Monica.Web.Module;
using Monica.Web.Options;
using Monica.Web.Util;
using System;

namespace Monica.Web
{
    public static class WebServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Monica框架对ASP.NET Core的支持
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebMonica(this IServiceCollection serviceCollection, Action<MonicaWebOptions> action = null)
        {
            serviceCollection.AddHttpContextAccessor();
            if (action == null)
            {
                var serviceProvider = serviceCollection.BuildServiceProvider();
                action = options => serviceProvider.GetService<IConfiguration>().GetSection("Framework:Web").Bind(options);
            }
            serviceCollection.Configure<MonicaWebOptions>(action);
            return serviceCollection;
        }

        /// <summary>
        /// 添加Monica框架对ASP.NET Core的支持
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebModule(this IServiceCollection serviceCollection, Action<MonicaWebOptions> action = null)
        {
            if (serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            var module = new AspNetCoreMonincaModule(action);
            serviceCollection.TryAddEnumerable(ServiceDescriptor.Singleton<IStartupModule>(module));
            return serviceCollection;
        }
    }
}
