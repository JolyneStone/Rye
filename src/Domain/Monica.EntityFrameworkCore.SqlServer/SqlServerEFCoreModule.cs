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
    public class SqlServerEFCoreModule<TContext> : IStartupModule
        where TContext : DbContext
    {
        public ModuleLevel Level => ModuleLevel.FrameWork;

        public uint Order => 4;

        public SqlServerEFCoreModule(string dbName, Action<DbContextOptionsBuilder<TContext>> action)
        {
            _dbName = dbName;
            _action = action;
        }

        private readonly string _dbName;
        private readonly Action<DbContextOptionsBuilder<TContext>> _action;

        public void ConfigueServices(IServiceCollection services)
        {
            services.AddMonicaSqlServer();
            services.AddDbBuilderOptions(_dbName, _action);
        }

        public void Configure(IServiceProvider serviceProvider)
        {
        }
    }
}
