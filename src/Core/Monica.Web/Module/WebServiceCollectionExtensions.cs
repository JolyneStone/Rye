using Microsoft.Extensions.DependencyInjection;
using Monica.Module;
using Monica.Web.Module;
using System;

namespace Monica
{
    public static class WebServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Monica框架对ASP.NET Core的支持
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddAspNetCoreMonica(this IServiceCollection serviceCollection)
        {
            if (serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            return serviceCollection.AddModule<AspNetCoreMonincaModule>();
        }
    }
}
