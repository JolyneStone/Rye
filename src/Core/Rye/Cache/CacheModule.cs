using Microsoft.Extensions.DependencyInjection;
using Rye.AspectFlare.DependencyInjection;
using Rye.Enums;
using Rye.Module;

namespace Rye.Cache
{
    /// <summary>
    /// 添加缓存模块
    /// </summary>
    //[DependsOnModules(typeof(AspectFlareModule))]
    public class CacheModule: StartupModule
    {
        public override ModuleLevel Level => ModuleLevel.Core;
        public override uint Order => 2;

        public override void ConfigueServices(IServiceCollection services)
        {
            services.AddCacheMemory();
        }
    }
}
