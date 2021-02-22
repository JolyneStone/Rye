using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Rye.Enums;
using Rye.Module;

using System;

namespace Rye.EntityFrameworkCore.MySql
{
    /// <summary>
    /// MySql 数据库EF Core模块
    /// </summary>
    //[DependsOnModules(typeof(EFCoreModule))]
    public class MySqlEFCoreModule : EFCoreModule, IStartupModule
    {
        //public ModuleLevel Level => ModuleLevel.FrameWork;

        //public uint Order => 4;

        public MySqlEFCoreModule(Action<RyeDbContextOptionsBuilder> action) : base(action)
        {

        }

        public override void ConfigueServices(IServiceCollection services)
        {
            base.ConfigueServices(services);
            services.AddRyeMySql();
        }

        public override void Configure(IServiceProvider serviceProvider)
        {
        }
    }
}
