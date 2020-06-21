using Raven.Jwt.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Raven.Jwt;

namespace Raven
{
    public static class JwtServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Raven JWT服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddRavenJwt(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IConfigureOptions<JwtOptions>, JwtOptionsSetup>();
            return AddRavenJwtCore(serviceCollection);
        }

        /// <summary>
        /// 添加Raven JWT服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddRavenJwt(this IServiceCollection serviceCollection, Action<JwtOptions> action)
        {
            serviceCollection.Configure<JwtOptions>(action);
            return AddRavenJwtCore(serviceCollection);
        }

        private static IServiceCollection AddRavenJwtCore(IServiceCollection serviceCollection)
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
                    opts.AddPolicy("RavenPermission", policy => policy.Requirements.Add(new RavenRequirement()));
                });
                serviceCollection.TryAddSingleton<IAuthorizationHandler, RavenPolicyAuthorizationHandler>();
            }
            serviceCollection.TryAddSingleton(typeof(IJwtTokenService<,,>), typeof(JwtTokenService<,,>));
            return serviceCollection;
        }
    }
}
