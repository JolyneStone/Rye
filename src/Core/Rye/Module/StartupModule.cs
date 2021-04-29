using Microsoft.Extensions.DependencyInjection;
using Rye.Enums;
using System;

namespace Rye.Module
{
    /// <summary>
    /// 启动模块基类
    /// </summary>
    public abstract class StartupModule : IStartupModule
    {
        public virtual ModuleLevel Level => ModuleLevel.Buiness;
        public virtual uint Order => 0;
        public virtual void Use(IServiceProvider serviceProvider)
        {
            return;
        }

        public virtual void ConfigueServices(IServiceCollection services)
        {
            return;
        }
    }
}
