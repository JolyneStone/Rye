using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Rye.Authorization.Abstraction;
using Rye.Authorization.Abstraction.Builder;
using Rye.Jwt.Options;

using System;

namespace Rye
{
    public static class AuthorizationServiceCollection
    {
        /// <summary>
        /// 添加Rye 授权服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddRyeAuthorization(this IServiceCollection serviceCollection, Action<IModuleAuthorizationBuilder> action)
        {
            Check.NotNull(action, nameof(action));

            JwtOptions jwtOptions;
            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    jwtOptions = scope.ServiceProvider.GetRequiredService<IOptions<JwtOptions>>().Value;
                }
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
