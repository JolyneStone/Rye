using Monica.Jwt.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Monica.Jwt;

namespace Monica
{
    public static class JwtServiceCollectionExtensions
    {
        /// <summary>
        /// 增加JWT模块
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwtModule(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddModule<JwtModule>();
        }

        /// <summary>
        /// 增加JWT模块
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwtModule(this IServiceCollection serviceCollection, Action<JwtOptions> action)
        {
            var module = new JwtModule(action);
            return serviceCollection.AddModule<JwtModule>(module);
        }

        /// <summary>
        /// 添加Monica JWT服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddMonicaJwt(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IConfigureOptions<JwtOptions>, JwtOptionsSetup>();
            return AddMonicaJwtCore(serviceCollection);
        }

        /// <summary>
        /// 添加Monica JWT服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddMonicaJwt(this IServiceCollection serviceCollection, Action<JwtOptions> action)
        {
            serviceCollection.Configure<JwtOptions>(action);
            return AddMonicaJwtCore(serviceCollection);
        }

        private static IServiceCollection AddMonicaJwtCore(IServiceCollection serviceCollection)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // 移除微软传统的声明映射方式，使用JWT映射方式
            using (var scope = serviceCollection.BuildServiceProvider().CreateScope())
            {
                var jwtOptions = scope.ServiceProvider.GetRequiredService<JwtOptions>();
                AuthenticationBuilder builder = serviceCollection.AddAuthentication(opts =>
                {
                    opts.DefaultAuthenticateScheme = jwtOptions.Scheme;
                    opts.DefaultChallengeScheme = jwtOptions.Scheme;
                })
                .AddJwtBearer(jwtOptions.Scheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)), // key值长度至少是16

                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,

                        ValidateLifetime = jwtOptions.IsExpire,
                        ClockSkew = TimeSpan.FromMinutes(jwtOptions.AccessExpireMins)
                    };
                });

                serviceCollection.AddAuthorization(opts =>
                {
                    opts.AddPolicy("MonicaPermission", policy => policy.Requirements.Add(new MonicaRequirement()));
                });
                serviceCollection.TryAddSingleton<IAuthorizationHandler, MonicaPolicyAuthorizationHandler>();
            }
            serviceCollection.TryAddSingleton(typeof(IJwtTokenService<,,>), typeof(JwtTokenService<,,>));
            return serviceCollection;
        }
    }
}
