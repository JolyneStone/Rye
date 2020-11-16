using Microsoft.Extensions.DependencyInjection;
using Monica.Cache;
using Monica.Enums;
using Monica.Module;
using Monica.Web.Options;
using System;

namespace Monica.Web.Module
{
    [DependsOnModules(typeof(MonicaCoreModule), typeof(CacheModule))]
    public class AspNetCoreMonincaModule: AspNetCoreModule
    {
        public override ModuleLevel Level => ModuleLevel.Core;
        public override uint Order => 1;

        private Action<MonicaWebOptions> _action;

        public AspNetCoreMonincaModule(Action<MonicaWebOptions> action = null)
        {
            _action = action;
        }

        public override void ConfigueServices(IServiceCollection services)
        {
            services.AddWebMonica(_action);
        }
    }
}
