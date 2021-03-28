using Rye;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Common.Enums
{
    public enum PermissionType
    {
        /// <summary>
        /// 一级菜单
        /// </summary>
        [Description("一级菜单"), LangKey("PermissionType.Module")]
        Module = 1,
        /// <summary>
        /// 二级菜单
        /// </summary>
        [Description("二级菜单"), LangKey("PermissionType.Menu")]
        Menu = 2,
        /// <summary>
        /// 权限
        /// </summary>
        [Description("权限"), LangKey("PermissionType.Authority")]
        Authority = 99
    }
}
