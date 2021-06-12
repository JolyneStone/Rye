using Demo.Common;
using Demo.Common.Enums;
using Demo.Model.Input;

using Microsoft.AspNetCore.Mvc;

using Rye;
using Rye.Authorization;
using Rye.Entities.Abstractions;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    [Route("v{v:apiVersion}/api/[controller]/[action]")]
    [Login]
    public class PermissionController:BaseController
    {


        /// <summary>
        /// 根据用户角色获取权限信息
        /// </summary>
        /// <param name="basicInput"></param>
        /// <param name="permissionService"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<object>> GetPermissions([FromQuery] BasicInput basicInput,
            [FromServices] IPermissionService permissionService)
        {
            var roleIdsStr = HttpContext.User.Claims.FirstOrDefault(d => d.Type.Equals("RoleIds", StringComparison.InvariantCultureIgnoreCase))?.Value;
            if (roleIdsStr.IsNullOrEmpty())
            {
                return Result<object>(DefaultStatusCode.UsertokenInvalid);
            }

            var permissons = await permissionService.GetPermissionCodeAsync(roleIdsStr);

            return Result<object>(DefaultStatusCode.Success,
             new
             {
                 Permissions = permissons
             });
        }
    }
}
