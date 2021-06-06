using Rye.Web.ResponseProvider.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Rye.Web.Options;
using Microsoft.AspNetCore.Http;
using Rye.Web;
using Microsoft.AspNetCore.Mvc;

namespace Rye.Authorization
{
    /// <summary>
    /// 限制登录用户访问
    /// </summary>
    public class LoginAttribute : AuthAttribute
    {
        public override int Priority => 1;

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
            if (!validResult.HasToken)
            {
                return Task.FromResult<(bool, JsonResult)>((false, Provider.CreateNotLoginResponse(httpContext)));
            }
            if (validResult.HasExpire)
            {
                return Task.FromResult<(bool, JsonResult)>((false, Provider.CreateTokenExpireResponse(httpContext)));
            }
            else if (!validResult.Success || httpContext.User == null)
            {
                return Task.FromResult<(bool, JsonResult)>((false, Provider.CreateTokenErrorResponse(httpContext)));
            }

            return Task.FromResult<(bool, JsonResult)>((true, null));
        }
    }
}
