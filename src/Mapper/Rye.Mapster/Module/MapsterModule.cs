using Microsoft.Extensions.DependencyInjection;

using Rye.Enums;
using Rye.Module;

using System.Reflection;

namespace Rye.Mapster
{
    /// <summary>
    /// 提供Mapster的初始化功能模块
    /// </summary>
    public class MapsterModule : StartupModule
    {
        public override ModuleLevel Level => ModuleLevel.FrameWork;

        public override uint Order => 9999;

        private readonly Assembly[] _assemblies;

        public MapsterModule(Assembly[] assemblies = null)
        {
            _assemblies = assemblies;
        }

        public override void ConfigueServices(IServiceCollection services)
        {
            services.AddMapster(_assemblies);
        }
    }
}
