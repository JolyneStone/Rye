using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Monica.Enums;
using Monica.Module;
using System;

namespace Monica.EntityFrameworkCore.MySql
{
    /// <summary>
    /// MySql 数据库EF Core模块
    /// </summary>
    //[DependsOnModules(typeof(EFCoreModule))]
    public class MySqlEFCoreModule<TContext> : EFCoreModule, IStartupModule
        where TContext: DbContext
    {
        //public ModuleLevel Level => ModuleLevel.FrameWork;

        //public uint Order => 4;

        public MySqlEFCoreModule(string dbName, Action<DbContextOptionsBuilder<TContext>> action)
        {
            _dbName = dbName;
            _action = action;
        }

        private readonly string _dbName;
        private readonly Action<DbContextOptionsBuilder<TContext>> _action;

        public override void ConfigueServices(IServiceCollection services)
        {
            services.AddMonicaMySql();
            services.AddDbBuilderOptions(_dbName, _action);
        }

        public override void Configure(IServiceProvider serviceProvider)
        {
        }
    }
}
