using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Monica.Authorization.Abstraction;
using Monica.Authorization.Abstraction.Builder;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monica
{
    public static class DefaultAuthorizationServiceCollection
    {
        /// <summary>
        /// 添加Monica 默认授权服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddMonicaAuthorization(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddMonicaAuthorization(ConfigureBuilder);
        }

        /// <summary>
        /// 增加默认授权模块
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthorizationModule(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddAuthorizationModule(ConfigureBuilder);
        }

        private static void ConfigureBuilder(IModuleAuthorizationBuilder builder)
        {
            builder.ConfigureOptions = options =>
            {
                options.InvokeHandlersAfterFailure = false;
                options.AddPolicy("MonicaPermission", policy => policy.Requirements.Add(new MonicaRequirement()));
            };
            builder.UseHandle<MonicaDefaultPolicyAuthorizationHandler>();
        }
    }
}
