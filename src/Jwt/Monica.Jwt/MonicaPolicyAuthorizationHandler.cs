using Monica.Jwt.Entities;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Monica.Jwt
{
    public class MonicaPolicyAuthorizationHandler: AuthorizationHandler<MonicaRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MonicaRequirement requirement)
        {
            var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext).HttpContext;
            var isAuthenticated = httpContext.User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                if(await AuthorizeCore(context))
                {
                    context.Succeed(requirement);
                }
            }
        }

        /// <summary>
        /// 根据用户角色进行权限认证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual Task<bool> AuthorizeCore(AuthorizationHandlerContext context)
        {
            // TODO: 根据用户角色进行权限认证
            return Task.FromResult(true);
        }
    }
}
