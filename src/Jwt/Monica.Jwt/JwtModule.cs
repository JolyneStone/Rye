using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Monica.Enums;
using Monica.Jwt.Options;
using Monica.Module;
using Monica.Web.Module;

using System;

namespace Monica.Jwt
{
    /// <summary>
    /// JWT 模块
    /// </summary>
    public class JwtModule : AspNetCoreModule
    {
        public override ModuleLevel Level => ModuleLevel.FrameWork;

        public override uint Order => 1;

        private readonly Action<JwtOptions> _action;

        public JwtModule()
        {
        }

        public JwtModule(Action<JwtOptions> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public override void ConfigueServices(IServiceCollection services)
        {
            if (_action != null)
            {
                services.AddMonicaJwt(_action);
            }
            else
            {
                services.AddMonicaJwt();
            }
        }

        public override void Configure(IApplicationBuilder app)
        {
        }
    }
}
