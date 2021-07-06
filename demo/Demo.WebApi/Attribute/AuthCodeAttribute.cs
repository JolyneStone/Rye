using Demo.Library.Abstraction;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Rye;
using Rye.Authorization;
using Rye.Authorization.Entities;
using Rye.Cache;
using Rye.Cache.Store;
using Rye.Web.Options;
using Rye.Web.ResponseProvider.Authorization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.WebApi
{
    /// <summary>
    /// 限制有权限的用户访问
    /// </summary>
    public class AuthCodeAttribute : AuthAttribute
    {
        public override int Priority => 2;

        /// <summary>
        /// 权限码，默认为Action名
        /// </summary>
        public string AuthCode { get; }

        public AuthCodeAttribute()
        {
        }

        public AuthCodeAttribute(string authCode)
        {
            AuthCode = authCode;
        }

        private static IAuthorizationResponseProvider _provider;

        private static IAuthorizationResponseProvider Provider
        {
            get
            {
                if (_provider != null)
                    return _provider;

                _provider = App.ApplicationServices.GetService<IOptions<RyeWebOptions>>().Value.Authorization.Provider;
                return _provider;
            }
        }

        public override async Task<(bool, JsonResult)> AuthorizeAsync(HttpContext httpContext, TokenValidResult validResult)
        {
            var url = httpContext.Request.Path.Value.ToLower();
            var roleIds = httpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(nameof(PermissionTokenEntity.RoleIds), StringComparison.InvariantCultureIgnoreCase))?.Value;
            var secutiryPermissionService = httpContext.RequestServices.GetService<IPermissionService>();
            if (secutiryPermissionService == null)
                return (true, null);

            if (roleIds.IsNullOrEmpty())
            {
                return (false, Provider.CreatePermissionNotAllowResponse(httpContext));
            }

            var userId = httpContext.User?.Claims.FirstOrDefault(c => c.Type.Equals(nameof(PermissionTokenEntity.UserId), StringComparison.InvariantCultureIgnoreCase))?.Value;
            if (userId.IsNullOrEmpty())
            {
                return (false, Provider.CreatePermissionNotAllowResponse(httpContext));
            }

            var store = httpContext.RequestServices.GetRequiredService<ICacheStore>();
            var entry = CacheEntryCollection.GetPermissionEntry(userId);
            IEnumerable<string> permissionList = await store.GetAsync<IEnumerable<string>>(entry);
            if (permissionList == null || !permissionList.Any())
            {
                permissionList = await secutiryPermissionService.GetPermissionCodeAsync(roleIds);
                if (permissionList != null && permissionList.Any())
                {
                    await store.SetAsync(entry, permissionList);
                }
            }

            var area = httpContext.GetRouteValue("area")?.ToString();
            var controller = httpContext.GetRouteValue("controller")?.ToString();
            var action = httpContext.GetRouteValue("action")?.ToString();
            var authCode = AuthCode ??
                (area != null ? $"{area}_{controller}.{action}" : $"{controller}.{action}");
            if (!permissionList.Any(d => string.Equals(d, authCode, System.StringComparison.InvariantCultureIgnoreCase)))
            {
                return (false, Provider.CreatePermissionNotAllowResponse(httpContext));
            }

            return (true, null);
        }
    }
}
