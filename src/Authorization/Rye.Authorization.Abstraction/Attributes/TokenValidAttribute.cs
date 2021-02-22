using Microsoft.AspNetCore.Authorization;

using System;

namespace Rye.Authorization.Abstraction.Attributes
{
    /// <summary>
    /// 限制错误的Token的访问
    /// </summary>
    public class TokenValidAttribute : AuthorizeAttribute
    {
        public TokenValidAttribute()
        {
            Policy = "RyePermission";
        }
    }
}
