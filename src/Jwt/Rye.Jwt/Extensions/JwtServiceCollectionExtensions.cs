using Rye.Jwt.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Rye.Jwt;
using Rye.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using System.Net.Http;

namespace Rye
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
        /// 添加Rye JWT服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwt(this IServiceCollection serviceCollection)
        {
            //serviceCollection.TryAddSingleton<IConfigureOptions<JwtOptions>, JwtOptionsSetup>();
            serviceCollection.Configure<JwtOptions>(ConfigurationManager.Appsettings.GetSection("Framework:Jwt"));
            return AddJwtCore(serviceCollection);
        }

        /// <summary>
        /// 添加Rye JWT服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwt(this IServiceCollection serviceCollection, Action<JwtOptions> action)
        {
            if (action != null)
            {
                serviceCollection.Configure<JwtOptions>(action);
            }
            else
            {
                serviceCollection.Configure<JwtOptions>(ConfigurationManager.Appsettings.GetSection("Framework:Jwt"));
            }
            return AddJwtCore(serviceCollection);
        }

        private static IServiceCollection AddJwtCore(IServiceCollection serviceCollection)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // 移除微软传统的声明映射方式，使用JWT映射方式
            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var jwtOptions = scope.ServiceProvider.GetRequiredService<IOptions<JwtOptions>>().Value;
                    AuthenticationBuilder builder = serviceCollection.AddAuthentication(opts =>
                    {
                        opts.DefaultAuthenticateScheme = jwtOptions.Scheme;
                        opts.DefaultChallengeScheme = jwtOptions.Scheme;
                    })
                    .AddJwtBearer(jwtOptions.Scheme, options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = jwtOptions.GetValidationParameters();
                        options.BackchannelHttpHandler = new HttpClientHandler()
                        {
                            UseDefaultCredentials = true,
                        };
                    //options.SecurityTokenValidators.Clear();
                    //options.SecurityTokenValidators.Add(new UserJwtSecurityTokenHandler());
                    options.Events = new JwtBearerEvents()
                        {
                            OnAuthenticationFailed = context =>
                            {
                                //Token expired
                                if (context.Exception is SecurityTokenExpiredException)
                                {
                                    context.Response.Headers.Add("token-expired", "true");
                                }
                                return Task.CompletedTask;
                            },
                            OnMessageReceived = context =>
                            {
                                if (jwtOptions.EnabledSignalR)
                                {  
                                    // 生成SignalR的用户信息
                                    string token = context.Request.Query["access_token"];
                                    string path = context.HttpContext.Request.Path;
                                    if (!string.IsNullOrEmpty(token) && path.Contains("hub"))
                                    {
                                        context.Token = token;
                                    }
                                }
                                return Task.CompletedTask;
                            }
                        };
                    });
                }
            }
            serviceCollection.TryAddSingleton<IJwtTokenService, JwtTokenService>();
            return serviceCollection;
        }
    }
}
