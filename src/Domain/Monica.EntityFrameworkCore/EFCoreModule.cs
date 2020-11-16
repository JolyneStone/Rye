using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Monica.DataAccess;
using Monica.EntityFrameworkCore.Options;
using Monica.Enums;
using Monica.Module;

using System;
using System.Collections.Generic;
using System.Text;

namespace Monica.EntityFrameworkCore
{
    [DependsOnModules(typeof(DataAccessModule))]
    public class EFCoreModule: StartupModule
    {
        public override ModuleLevel Level => ModuleLevel.FrameWork;
        public override uint Order => 3;

        private Action<DbContextOptionsBuilderOptions> _action;

        public EFCoreModule(Action<DbContextOptionsBuilderOptions> action = null)
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
