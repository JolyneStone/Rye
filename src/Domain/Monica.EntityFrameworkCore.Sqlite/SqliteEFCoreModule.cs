using Microsoft.Extensions.DependencyInjection;
using Monica.Enums;
using Monica.Module;
using System;

namespace Monica.EntityFrameworkCore.Sqlite
{
    /// <summary>
    /// Sqlite 数据库EF Core模块
    /// </summary>
    [DependsOnModules(typeof(EFCoreModule))]
    public class SqliteEFCoreModule : IStartupModule
    {
        public ModuleLevel Level => ModuleLevel.FrameWork;

        public uint Order => 4;

        public void ConfigueServices(IServiceCollection services)
        {
            services.AddMonicaSqlite();
        }

        public void Configure(IServiceProvider serviceProvider)
        {
        }
    }
}
