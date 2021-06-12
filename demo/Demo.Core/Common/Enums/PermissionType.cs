using Rye;

using System.ComponentModel;

namespace Demo.Common.Enums
{
    public enum PermissionType
    {
        /// <summary>
        /// 一级菜单
        /// </summary>
        [Description("一级菜单"), Lang("PermissionType.Module", "一级菜单")]
        Module = 1,
        /// <summary>
        /// 二级菜单
        /// </summary>
        [Description("二级菜单"), Lang("PermissionType.Menu", "二级菜单")]
        Menu = 2,
        /// <summary>
        /// 权限
        /// </summary>
        [Description("权限"), Lang("PermissionType.Authority", "权限")]
        Authority = 99
    }
}
