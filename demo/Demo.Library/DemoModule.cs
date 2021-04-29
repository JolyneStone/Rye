using Demo.Library.Business;

using Microsoft.Extensions.DependencyInjection;

using Rye;
using Rye.Enums;
using Rye.Module;
using System;

namespace Demo.Library
{
    public class DemoModule: StartupModule
    {
        public override uint Order => 1;
        public override ModuleLevel Level => ModuleLevel.Buiness;

        public override void ConfigueServices(IServiceCollection services)
        {
            services.AddSecuritySupport<SecurityService>();
        }

        public override void Use(IServiceProvider serviceProvider)
        {
        }
    }
}
