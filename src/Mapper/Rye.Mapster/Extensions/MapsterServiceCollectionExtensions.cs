using Mapster;

using MapsterMapper;

using Microsoft.Extensions.DependencyInjection;

using Rye.Mapster;

using System.Reflection;

namespace Rye
{
    public static class MapsterServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Mapster对象映射
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="assemblies">扫描的程序集</param>
        /// <returns></returns>
        public static IServiceCollection AddMapster(this IServiceCollection services, Assembly[] assemblies = null)
        {
            // 获取全局映射配置
            var config = TypeAdapterConfig.GlobalSettings;

            // 扫描所有继承  IRegister 接口的对象映射配置
            if (assemblies != null && assemblies.Length > 0) config.Scan(assemblies ?? App.Assemblies);

            // 配置默认全局映射（支持覆盖）
            config.Default
                  .NameMatchingStrategy(NameMatchingStrategy.Flexible)
                  .PreserveReference(true);

            // 配置支持依赖注入
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }

        /// <summary>
        /// 添加Mapster对象映射模块
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddMapsterModule(this IServiceCollection services, Assembly[] assemblies = null)
        {
            var module = new MapsterModule(assemblies);
            return services.AddModule(module);
        }
    }
}
