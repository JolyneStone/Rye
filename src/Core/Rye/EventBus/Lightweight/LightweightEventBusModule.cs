using Microsoft.Extensions.DependencyInjection;
using Rye.Enums;
using Rye.EventBus.Lightweight.Options;
using Rye.Module;
using System;

namespace Rye.EventBus.Lightweight
{
    /// <summary>
    /// 适用于内存的事件总线模块
    /// </summary>
    public class LightweightEventBusModule : StartupModule
    {
        public override ModuleLevel Level => ModuleLevel.FrameWork;

        public override uint Order => 2;

        private readonly Action<LightweightEventBusOptions> _action;

        public LightweightEventBusModule(Action<LightweightEventBusOptions> action)
        {
            _action = action;
        }

        public override void ConfigueServices(IServiceCollection services)
        {
            services.AddLightweightEventBus(_action);
        }

    }
}
