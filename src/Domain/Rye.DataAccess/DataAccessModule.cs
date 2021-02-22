using Microsoft.Extensions.DependencyInjection;
using Rye.DataAccess.Options;
using Rye.Enums;
using Rye.Module;
using System;

namespace Rye.DataAccess
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
