using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Monica.Authorization.Abstraction.Builder;
using Monica.Cache;
using Monica.DataAccess;
using Monica.Enums;
using Monica.Module;
using Monica.Web.Module;

using System;

namespace Monica.Authorization.Abstraction
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
            services.AddMonicaAuthorization(_action);
        }

        public override void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();//身份验证
            app.UseAuthorization();// 授权
        }
    }
}
