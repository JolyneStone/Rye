using Microsoft.Extensions.DependencyInjection;
using Monica.Enums;
using Monica.Module;
using System;

namespace Demo.Library
{
    public class DemoModule: StartupModule
    {
        public override uint Order => 1;
        public override ModuleLevel Level => ModuleLevel.Application;

        public override void ConfigueServices(IServiceCollection services)
        {
        }

        public override void Configure(IServiceProvider serviceProvider)
        {
        }
    }
}
