using Microsoft.Extensions.DependencyInjection;
using Monica.Enums;
using Monica.Module;
using System;

namespace Monica.EntityFrameworkCore.MySql
{
    /// <summary>
    /// MySql 数据库EF Core模块
    /// </summary>
    [DependsOnModules(typeof(EFCoreModule))]
    public class MySqlEFCoreModule : IStartupModule
    {
        public ModuleLevel Level => ModuleLevel.FrameWork;

        public uint Order => 4;

        public void ConfigueServices(IServiceCollection services)
        {
            services.AddMonicaMySql();
        }

        public void Configure(IServiceProvider serviceProvider)
        {
        }
    }
}
