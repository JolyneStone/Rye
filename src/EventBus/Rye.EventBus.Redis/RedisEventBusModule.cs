using Microsoft.Extensions.DependencyInjection;

using Rye.Enums;
using Rye.EventBus.Redis.Options;
using Rye.Module;

using System;

namespace Rye.EventBus.Redis
{
    /// <summary>
    /// 适用于Redis的事件总线模块
    /// </summary>
    public class RedisEventBusModule : StartupModule
    {
        public override ModuleLevel Level => ModuleLevel.FrameWork;

        public override uint Order => 2;

        private readonly Action<RedisEventBusOptions> _action;

        public RedisEventBusModule(Action<RedisEventBusOptions> action)
        {
            _action = action;
        }

        public override void ConfigueServices(IServiceCollection services)
        {
            services.AddRedisEventBus(_action);
        }

        public override void Use(IServiceProvider serviceProvider)
        {
            serviceProvider.ApplyRedisSubscriberHandler();
        }
    }
}
