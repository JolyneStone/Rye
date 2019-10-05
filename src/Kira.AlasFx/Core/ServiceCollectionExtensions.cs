using Kira.AlasFx.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Kira.AlasFx.Core
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加AlasFx框架
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddAlasFx(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddOptions();
            serviceCollection.TryAddSingleton<IConfigureOptions<AlasFxOptions>, AlasFxOptionsSetup>();
            return serviceCollection;
        }
    }
}
