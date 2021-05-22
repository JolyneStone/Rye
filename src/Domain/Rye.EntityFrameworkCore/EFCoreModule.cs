using Microsoft.Extensions.DependencyInjection;

using Rye.DataAccess;
using Rye.Enums;
using Rye.Module;

using System;

namespace Rye.EntityFrameworkCore
{
    [DependsOnModules(typeof(DataAccessModule))]
    public abstract class EFCoreModule: StartupModule
    {
        public override ModuleLevel Level => ModuleLevel.FrameWork;
        public override uint Order => 3;

        protected readonly Action<RyeDbContextOptionsBuilder> _action;

        public EFCoreModule(Action<RyeDbContextOptionsBuilder> action = null)
        {
            _action = action;
        }

        public override void ConfigueServices(IServiceCollection services)
        {
            services.AddEFCoreDatabase();
            services.AddDbBuillderOptions(_action);
        }
    }
}
