using Microsoft.Extensions.DependencyInjection;
using Rye.DataAccess;
using Rye.Enums;
using Rye.Module;
using System;

namespace Rye.MySql
{
    /// <summary>
    /// 提供对MySql数据库访问的功能模块
    /// </summary>
    [DependsOnModules(typeof(DataAccessModule))]
    public class MySqlModule : IStartupModule
    {
        public ModuleLevel Level => ModuleLevel.FrameWork;

        public uint Order => 3;

        public MySqlModule(Type providerType)
        {
            _providerType = providerType;
        }

        public MySqlModule(Func<IServiceProvider, object> providerFunc)
        {
            _providerFunc = providerFunc;
        }

        private Type _providerType;
        private Func<IServiceProvider, object> _providerFunc;


        public void ConfigueServices(IServiceCollection services)
        {
            if (_providerFunc != null)
            {
                services.AddMySqlDbConnectionProvider(_providerFunc);
            }
            else if (_providerType != null)
            {
                services.AddMySqlDbConnectionProvider(_providerType);
            }
        }

        public void Configure(IServiceProvider serviceProvider)
        {
        }
    }
}
