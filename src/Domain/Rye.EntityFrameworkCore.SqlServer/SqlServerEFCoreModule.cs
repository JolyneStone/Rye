using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Rye.Enums;
using Rye.Module;

using System;

namespace Rye.EntityFrameworkCore.SqlServer
{
    /// <summary>
    /// Sql Server 数据库EF Core模块
    /// </summary>
    [DependsOnModules(typeof(EFCoreModule))]
    public class SqlServerEFCoreModule : EFCoreModule
    {
        //public ModuleLevel Level => ModuleLevel.FrameWork;

        //public uint Order => 4;

        public SqlServerEFCoreModule(Action<RyeDbContextOptionsBuilder> action) : base(action)
        {

        }

        public override void ConfigueServices(IServiceCollection services)
        {
            base.ConfigueServices(services);
            services.AddSqlServerEFCore();
        }

        public override void Use(IServiceProvider serviceProvider)
        {
        }
    }
}
