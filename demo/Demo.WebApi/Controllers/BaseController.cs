using Demo.Core.Common;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using Rye.Business.Language;

using System;

namespace Demo.WebApi.Controllers
{
    [ApiController]
    [Validation]
    public abstract class BaseController : Controller
    {
        private ILangService _langService;
        protected ILangService LangService
        {
            get
            {
                if (_langService != null)
                    return _langService;
                _langService = HttpContext.RequestServices.GetRequiredService<ILangService>();
                return _langService;
            }
        }

        private string _ipAddress;
        protected string IpAddress
        {
            get
            {
                if (_ipAddress != null)
                    return _ipAddress;
                _ipAddress = Rye.Web.Util.IpAddress.GetRemoteIpV4AddressStr(HttpContext);
                return _ipAddress;
            }
        }

        private string _userAgent;
        protected string UserAgent
        {
            get
            {
                if (_userAgent != null)
                    return _userAgent;
                _userAgent = Request.Headers["User-Agent"];
                return _userAgent;
            }
        }

        private string _lang;
        protected string Lang
        {
            get
            {
                if (_lang != null)
                    return _lang;
                if (Request.Query.TryGetValue("lang", out var val))
                {
                    _lang = val.ToString();
                }

                if (string.IsNullOrEmpty(_lang))
                    _lang = LangCode.ZHCN;

                return _lang;
            }
        }

        protected ApiResult Result(Enum @enum, string msg = null)
        {
            if (msg == null)
            {
                msg = LangService.Get(Lang, @enum);
            }
            return ApiResult.Create(@enum.GetHashCode(), msg);
        }

        protected ApiResult<T> Result<T>(Enum @enum, T data = default, string msg = null)
        {
            if (msg == null)
            {
                msg = LangService.Get(Lang, @enum);
            }
            return ApiResult<T>.Create(@enum.GetHashCode(), data, msg);
        }
    }
}
