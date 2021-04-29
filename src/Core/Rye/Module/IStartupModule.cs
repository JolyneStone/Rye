using Microsoft.Extensions.DependencyInjection;
using Rye.Enums;
using System;

namespace Rye.Module
{
    public interface IStartupModule
    {
        /// <summary>
        /// 模块级别，级别越小，优先级越高
        /// </summary>
        ModuleLevel Level { get; }
        /// <summary>
        /// 启动顺序，同级别模块此值越小，优先级越高
        /// </summary>
        uint Order { get; }
        /// <summary>
        /// 配置模块依赖注入
        /// </summary>
        /// <param name="services"></param>
        void ConfigueServices(IServiceCollection services);
        /// <summary>
        /// 配置模块服务
        /// </summary>
        /// <param name="serviceProvider"></param>
        void Use(IServiceProvider serviceProvider);
    }
}
