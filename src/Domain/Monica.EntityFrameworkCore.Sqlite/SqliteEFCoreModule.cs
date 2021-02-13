using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Monica.Enums;
using Monica.Module;
using System;

namespace Monica.EntityFrameworkCore.Sqlite
{
    /// <summary>
    /// Sqlite 数据库EF Core模块
    /// </summary>
    //[DependsOnModules(typeof(EFCoreModule))]
    public class SqliteEFCoreModule : EFCoreModule, IStartupModule
    {
        //public ModuleLevel Level => ModuleLevel.FrameWork;

        //public uint Order => 4;

        public SqliteEFCoreModule(Action<MonicaDbContextOptionsBuilder> action) : base(action)
        {

        }

        public override void ConfigueServices(IServiceCollection services)
        {
            base.ConfigueServices(services);
            services.AddMonicaSqlite();
        }

        public override void Configure(IServiceProvider serviceProvider)
        {
        }
    }
}
