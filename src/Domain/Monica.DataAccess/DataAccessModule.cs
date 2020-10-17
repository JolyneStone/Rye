using Microsoft.Extensions.DependencyInjection;
using Monica.DataAccess.Options;
using Monica.Enums;
using Monica.Module;
using System;

namespace Monica.DataAccess
{
    /// <summary>
    /// 数据访问层模块
    /// </summary>
    public class DataAccessModule : StartupModule
    {
        public override ModuleLevel Level => ModuleLevel.FrameWork;
        public override uint Order => 2;

        private Action<DbConnectionMapOptions> _action;

        public DataAccessModule(Action<DbConnectionMapOptions> action = null)
        {
            _action = action;
        }

        public override void ConfigueServices(IServiceCollection services)
        {
            services.AddDbConnections(_action);
        }
    }
}
