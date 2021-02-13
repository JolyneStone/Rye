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
    public abstract class EFCoreModule: StartupModule
    {
        public override ModuleLevel Level => ModuleLevel.FrameWork;
        public override uint Order => 3;

        protected readonly Action<MonicaDbContextOptionsBuilder> _action;

        public EFCoreModule(Action<MonicaDbContextOptionsBuilder> action = null)
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
