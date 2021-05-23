using Microsoft.Extensions.DependencyInjection;
using Rye.DependencyInjection;
using Rye.Enums;
using Rye.Options;
using System;

namespace Rye.Module
{
    /// <summary>
    /// Rye框架核心模块
    /// </summary>
    public class RyeCoreModule : StartupModule
    {
        public override ModuleLevel Level => ModuleLevel.Core;
        public override uint Order => 0;

        private IServiceCollection Services;

        private Action<RyeOptions> _action;

        public RyeCoreModule(Action<RyeOptions> action = null)
        {
            _action = action;
        }

        public override void ConfigueServices(IServiceCollection services)
        {
            Services = services;
            services.AddRye(_action);
        }

        public override void Use(IServiceProvider serviceProvider)
        {
            App.ConfigureServiceLocator(serviceProvider);
            //SingleServiceLocator.ConfigService(serviceProvider);
        }
    }
}
