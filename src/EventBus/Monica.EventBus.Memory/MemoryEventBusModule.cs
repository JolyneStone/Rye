using Microsoft.Extensions.DependencyInjection;
using Monica.Enums;
using Monica.Module;
using System;

namespace Monica.EventBus.Memory
{
    /// <summary>
    /// 内存间事件总线模块，适用于单体应用
    /// </summary>
    public class MemoryEventBusModule : IStartupModule
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
