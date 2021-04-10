using Microsoft.Extensions.DependencyInjection;

using Rye.Business.Options;
using Rye.Enums;
using Rye.Module;

using System;

namespace Rye.Business
{
    public class RyeBusinessModule : StartupModule
    {
        public override ModuleLevel Level => ModuleLevel.Buiness;
        public override uint Order => 1;

        private Action<BusinessOptions> _action;

        public RyeBusinessModule(Action<BusinessOptions> action = null)
        {
            _action = action;
        }

        public override void ConfigueServices(IServiceCollection services)
        {
            services.AddBusiness(_action);
        }
    }
}
