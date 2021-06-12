using Demo.Common;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using Rye;

using System;

namespace Demo.WebApi.Controllers
{
    [ApiController]
    [Validation]
    public abstract class BaseController : Controller
    {
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

        protected ApiResult Result(Enum @enum, string msg = null)
        {
            if (msg == null)
            {
                msg = I18n.GetText(@enum);
            }
            return ApiResult.Create(@enum.GetHashCode(), msg);
        }

        protected ApiResult<T> Result<T>(Enum @enum, T data = default, string msg = null)
        {
            if (msg == null)
            {
                msg = I18n.GetText(@enum);
            }
            return ApiResult<T>.Create(@enum.GetHashCode(), data, msg);
        }
    }
}
