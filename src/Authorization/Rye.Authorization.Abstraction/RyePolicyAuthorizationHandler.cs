using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;

using Rye.Web.Options;

using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Rye.Jwt.Options;
using System;
using System.Text;
using System.Security.Claims;

namespace Rye.Authorization.Abstraction
{
    public abstract class RyePolicyAuthorizationHandler : AuthorizationHandler<RyeRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RyeRequirement requirement)
        {
            var httpContext = context.Resource as HttpContext;
            if (httpContext.Request.Headers.TryGetValue("auth-res", out var res))
            {
                var str = res.ToString();
                if (!str.IsNullOrEmpty())
                {
                    if (str == "1")
                    {
                        context.Succeed(requirement);
                        return;
                    }
                    else
                    {
                        context.Fail();
                        return;
                    }
                }
            }

            if (await AuthorizeCoreAsync(httpContext))
            {
                httpContext.Request.Headers.Add("auth-res", "1");
                context.Succeed(requirement);
                return;
            }
            httpContext.Request.Headers.Add("auth-res", "0");
            context.Fail();
        }

        /// <summary>
        /// 根据用户角色进行权限认证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual Task<bool> AuthorizeCoreAsync(HttpContext context)
        {
            return Task.FromResult(true);
        }
    }
}
