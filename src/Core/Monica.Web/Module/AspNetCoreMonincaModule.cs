using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Monica.Enums;
using Monica.Web.Util;

namespace Monica.Web.Module
{
    public class AspNetCoreMonincaModule: AspNetCoreModule
    {
        public override ModuleLevel Level => ModuleLevel.Core;
        public override uint Order => 1;

        public override void ConfigueServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.TryAddScoped<IpAddress, IpAddress>();
        }
    }
}
