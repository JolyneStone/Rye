using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rye.Authorization
{
    /// <summary>
    /// 认证授权特性，其它的认证特性需继承此类
    /// </summary>
    public abstract class AuthAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 执行顺序优先级，按升序排序
        /// </summary>
        public abstract int Priority { get; }

        public AuthAttribute() : base(policy: "RyeAuth")
        {
        }

        public AuthAttribute(string policy) : base(policy: policy)
        {
        }

        public abstract Task<(bool, JsonResult)> AuthorizeAsync(HttpContext httpContext, TokenValidResult validResult);
    }
}
