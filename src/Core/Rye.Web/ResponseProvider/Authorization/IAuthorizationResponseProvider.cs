using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rye.Web.ResponseProvider.Authorization
{
    public interface IAuthorizationResponseProvider
    {
        /// <summary>
        /// 创建未登录响应
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        JsonResult CreateNotLoginResponse(HttpContext context);
        
        /// <summary>
        /// 创建Token过期响应
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        JsonResult CreateTokenExpireResponse(HttpContext context);
        
        /// <summary>
        /// 创建权限不足响应
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        JsonResult CreatePermissionNotAllowResponse(HttpContext context);

        /// <summary>
        /// 创建Token 错误响应
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        JsonResult CreateTokenErrorResponse(HttpContext context);
    }
}
