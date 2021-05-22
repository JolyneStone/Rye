using Microsoft.Extensions.DependencyInjection;
using Rye.Enums;
using Rye.EventBus.InMemory.Options;
using Rye.Module;
using System;

namespace Rye.EventBus.InMemory
{
    /// <summary>
    /// 适用于内存的事件总线模块
    /// </summary>
    public class InMemoryEventBusModule : StartupModule
    {
        public override ModuleLevel Level => ModuleLevel.FrameWork;

        public override uint Order => 2;

        private readonly Action<InMemoryEventBusOptions> _action;

        public InMemoryEventBusModule(Action<InMemoryEventBusOptions> action)
        {
            _action = action;
        }

        public override void ConfigueServices(IServiceCollection services)
        {
            services.AddInMemoryEventBus(_action);
        }

    }
}
