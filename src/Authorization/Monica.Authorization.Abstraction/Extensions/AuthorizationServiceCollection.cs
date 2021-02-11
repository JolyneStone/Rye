using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using Monica.Authorization.Abstraction;
using Monica.Authorization.Abstraction.Builder;
using Monica.Jwt.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monica
{
    public static class AuthorizationServiceCollection
    {
        /// <summary>
        /// 添加Monica 授权服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddMonicaAuthorization(this IServiceCollection serviceCollection, Action<IModuleAuthorizationBuilder> action)
        {
            Check.NotNull(action, nameof(action));

            JwtOptions jwtOptions;
            using(var scope = serviceCollection.BuildServiceProvider().CreateScope())
            {
                jwtOptions = scope.ServiceProvider.GetRequiredService<IOptions<JwtOptions>>().Value;
            }

            var builder = new ModuleAuthorizationBuilder(serviceCollection);
            action(builder);
            serviceCollection.AddAuthorization(opts =>
            {
                builder.ConfigureOptions?.Invoke(opts);
            })
            .AddAuthentication(options =>
            {
                options.DefaultScheme = jwtOptions.Scheme;
                options.DefaultChallengeScheme = jwtOptions.Scheme;
                options.DefaultAuthenticateScheme = jwtOptions.Scheme;
                options.DefaultForbidScheme = jwtOptions.Scheme;
                options.DefaultSignInScheme = jwtOptions.Scheme;
                options.DefaultSignOutScheme = jwtOptions.Scheme;
            });
            return serviceCollection;
        }

        /// <summary>
        /// 增加授权模块
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthorizationModule(this IServiceCollection serviceCollection, Action<IModuleAuthorizationBuilder> action)
        {
            Check.NotNull(action, nameof(action));
            var module = new AuthorizationModule(action);
            return serviceCollection.AddModule<AuthorizationModule>(module);
        }
    }
}
