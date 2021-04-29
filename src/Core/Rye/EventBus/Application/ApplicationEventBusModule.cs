using Microsoft.Extensions.DependencyInjection;
using Rye.Enums;
using Rye.EventBus.Application.Options;
using Rye.Module;
using System;

namespace Rye.EventBus.Application
{
    /// <summary>
    /// 适用于单体应用的事件总线模块，适用于单体应用
    /// </summary>
    public class ApplicationEventBusModule : IStartupModule
    {
        public ModuleLevel Level => ModuleLevel.FrameWork;

        public uint Order => 2;

        private readonly Action<ApplicationEventBusOptions> _action;

        public ApplicationEventBusModule(Action<ApplicationEventBusOptions> action)
        {
            _action = action;
        }

        public void ConfigueServices(IServiceCollection services)
        {
            services.AddApplicationEventBus(_action);
        }

        public void Use(IServiceProvider serviceProvider)
        {
        }
    }
}
