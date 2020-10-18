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
    public class SqlServerEFCoreModule : IStartupModule
    {
        public ModuleLevel Level => ModuleLevel.FrameWork;

        public uint Order => 4;

        public void ConfigueServices(IServiceCollection services)
        {
            services.AddMonicaSqlServer();
        }

        public void Configure(IServiceProvider serviceProvider)
        {
        }
    }
}
