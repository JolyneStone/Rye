using Microsoft.Extensions.DependencyInjection;

using Rye.Enums;
using Rye.EventBus.RabbitMQ.Options;
using Rye.Module;

using System;

namespace Rye.EventBus.RabbitMQ
{
    /// <summary>
    /// 适用于Rabbit MQ的事件总线
    /// </summary>
    public class RabbitMQEventBusModule : StartupModule
    {
        public override ModuleLevel Level => ModuleLevel.FrameWork;

        public override uint Order => 2;

        private readonly Action<RabbitMQEventBusOptions> _action;

        public RabbitMQEventBusModule(Action<RabbitMQEventBusOptions> action)
        {
            _action = action;
        }

        public override void ConfigueServices(IServiceCollection services)
        {
            services.AddRabbitMQEventBus(_action);
        }

        public override void Use(IServiceProvider serviceProvider)
        {
            serviceProvider.ApplyRabbitMQSubscriberHandler();
        }
    }
}
