using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Rye.Authorization.Abstraction.Builder;
using Rye.Cache;
using Rye.DataAccess;
using Rye.Enums;
using Rye.Module;
using Rye.Web.Module;

using System;

namespace Rye.Authorization.Abstraction
{
    /// <summary>
    /// 授权模块
    /// </summary>
    [DependsOnModules(typeof(DataAccessModule), typeof(CacheModule))]
    public class AuthorizationModule : AspNetCoreModule
    {
        public override ModuleLevel Level => ModuleLevel.FrameWork;

        public override uint Order => 2;

        private readonly Action<IModuleAuthorizationBuilder> _action;

        public AuthorizationModule(Action<IModuleAuthorizationBuilder> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public override void ConfigueServices(IServiceCollection services)
        {
            services.AddRyeAuthorization(_action);
        }

        public override void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();//身份验证
            app.UseAuthorization();// 授权
        }
    }
}
