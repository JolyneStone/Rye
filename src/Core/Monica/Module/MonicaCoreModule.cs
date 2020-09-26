using Microsoft.Extensions.DependencyInjection;
using Monica.DependencyInjection;
using Monica.Enums;
using Monica.Options;
using System;

namespace Monica.Module
{
    /// <summary>
    /// Monica框架核心模块
    /// </summary>
    public class MonicaCoreModule : StartupModule
    {
        public override ModuleLevel Level => ModuleLevel.Core;
        public override uint Order => 0;

        private IServiceCollection Services;

        private Action<MonicaOptions> _action;

        public MonicaCoreModule(Action<MonicaOptions> action = null)
        {
            _action = action;
        }

        public override void ConfigueServices(IServiceCollection services)
        {
            Services = services;
            services.AddMonica(_action);
        }

        public override void Configure(IServiceProvider serviceProvider)
        {
            SingleServiceLocator.SetServiceCollection(Services);
        }
    }
}
