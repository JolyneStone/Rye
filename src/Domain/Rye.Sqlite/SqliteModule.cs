using Microsoft.Extensions.DependencyInjection;
using Rye.DataAccess;
using Rye.Enums;
using Rye.Module;
using System;

namespace Rye.Sqlite
{
    /// <summary>
    /// 提供对Sqlite数据库访问的功能模块
    /// </summary>
    [DependsOnModules(typeof(DataAccessModule))]
    public class SqliteModule : IStartupModule
    {
        public ModuleLevel Level => ModuleLevel.FrameWork;

        public uint Order => 3;

        public SqliteModule(Type providerType)
        {
            _providerType = providerType;
        }

        public SqliteModule(Func<IServiceProvider, object> providerFunc)
        {
            _providerFunc = providerFunc;
        }

        private Type _providerType;
        private Func<IServiceProvider, object> _providerFunc;


        public void ConfigueServices(IServiceCollection services)
        {
            if (_providerFunc != null)
            {
                services.AddSqliteDbConnectionProvider(_providerFunc);
            }
            else if (_providerType != null)
            {
                services.AddSqliteDbConnectionProvider(_providerType);
            }
        }

        public void Configure(IServiceProvider serviceProvider)
        {
        }
    }
}
