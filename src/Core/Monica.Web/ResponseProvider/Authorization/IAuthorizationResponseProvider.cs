using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monica.Web.ResponseProvider.Authorization
{
    public interface IAuthorizationResponseProvider
    {
        /// <summary>
        /// 创建未登录响应
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        JsonResult CreateNotLoginResponse(AuthorizationResponseContext context);
        
        /// <summary>
        /// 创建Token过期响应
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        JsonResult CreateTokenExpireResponse(AuthorizationResponseContext context);
        
        /// <summary>
        /// 创建权限不足响应
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        JsonResult CreatePermissionNotAllowResponse(AuthorizationResponseContext context);

        /// <summary>
        /// 创建Token 错误响应
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        JsonResult CreateTokenErrorResponse(AuthorizationResponseContext context);
    }
}
