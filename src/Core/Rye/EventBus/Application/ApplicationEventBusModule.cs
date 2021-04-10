using Microsoft.Extensions.DependencyInjection;
using Rye.Enums;
using Rye.Module;
using System;

namespace Rye.EventBus.Application
{
    /// <summary>
    /// 内存间事件总线模块，适用于单体应用
    /// </summary>
    public class ApplicationEventBusModule : IStartupModule
    {
        public ModuleLevel Level => ModuleLevel.FrameWork;

        public uint Order => 2;

        public void ConfigueServices(IServiceCollection services)
        {
            services.AddMemoryEventBus();
        }

        public void Configure(IServiceProvider serviceProvider)
        {
        }
    }
}
