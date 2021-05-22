using DotNetCore.CAP;

using Microsoft.Extensions.DependencyInjection;

using Rye.Enums;
using Rye.Module;

using System;

namespace Rye.EventBus.CAP
{
    /// <summary>
    /// 添加Cap分布式事件总线模块的支持
    /// </summary>
    public class CapEventBusModule : StartupModule
    {
        public override ModuleLevel Level => ModuleLevel.FrameWork;

        public override uint Order => 2;

        private readonly Action<CapOptions> _action;

        public CapEventBusModule(Action<CapOptions> action)
        {
            _action = action;
        }

        public override void ConfigueServices(IServiceCollection services)
        {
            services.AddCapEventBus(_action);
        }
    }
}
