using Microsoft.Extensions.DependencyInjection;
using Rye.DataAccess;
using Rye.Enums;
using Rye.Module;
using System;

namespace Rye.SqlServer
{
    /// <summary>
    /// 提供对Sql Server数据库访问的功能模块
    /// </summary>
    [DependsOnModules(typeof(DataAccessModule))]
    public class SqlServerModule : StartupModule
    {
        public override ModuleLevel Level => ModuleLevel.FrameWork;

        public override uint Order => 3;

        public SqlServerModule(Type providerType)
        {
            _providerType = providerType;
        }

        public SqlServerModule(Func<IServiceProvider, object> providerFunc)
        {
            _providerFunc = providerFunc;
        }

        private Type _providerType;
        private Func<IServiceProvider, object> _providerFunc;


        public override void ConfigueServices(IServiceCollection services)
        {
            if (_providerFunc != null)
            {
                services.AddSqlServerDbConnectionProvider(_providerFunc);
            }
            else if (_providerType != null)
            {
                services.AddSqlServerDbConnectionProvider(_providerType);
            }
        }
    }
}
