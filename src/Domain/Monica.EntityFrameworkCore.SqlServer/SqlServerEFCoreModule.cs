using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Monica.Enums;
using Monica.Module;

using System;

namespace Monica.EntityFrameworkCore.SqlServer
{
    /// <summary>
    /// Sql Server 数据库EF Core模块
    /// </summary>
    [DependsOnModules(typeof(EFCoreModule))]
    public class SqlServerEFCoreModule : EFCoreModule, IStartupModule
    {
        //public ModuleLevel Level => ModuleLevel.FrameWork;

        //public uint Order => 4;

        public SqlServerEFCoreModule(Action<MonicaDbContextOptionsBuilder> action) : base(action)
        {

        }

        public override void ConfigueServices(IServiceCollection services)
        {
            base.ConfigueServices(services);
            services.AddMonicaSqlServer();
        }

        public override void Configure(IServiceProvider serviceProvider)
        {
        }
    }
}
