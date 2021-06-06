using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Rye.Web.Options;
using Rye.Web.ResponseProvider.Authorization;

using System.Threading.Tasks;

namespace Rye.Authorization
{
    /// <summary>
    /// 限制错误的Token的访问
    /// </summary>
    public class TokenValidAttribute : AuthAttribute
    {
        public override int Priority => 0;

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

        public override Task<(bool, JsonResult)> AuthorizeAsync(HttpContext httpContext, TokenValidResult validResult)
        {
            if (!validResult.HasToken || validResult.Success)
                return Task.FromResult<(bool, JsonResult)>((true, null));

            if (validResult.HasExpire)
            {
                return Task.FromResult<(bool, JsonResult)>((false, Provider.CreateTokenExpireResponse(httpContext)));
            }
            else if (!validResult.Success)
            {
                return Task.FromResult<(bool, JsonResult)>((false, Provider.CreateTokenErrorResponse(httpContext)));
            }

            return Task.FromResult<(bool, JsonResult)>((true, null));
        }
    }
}
