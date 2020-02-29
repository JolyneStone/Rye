using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Kira.AlasFx.Entities
{
    /// <summary>
    /// 权限类型
    /// </summary>
    public enum AuthorityType : byte
    {
        /// <summary>
        /// 系统
        /// </summary>
        [Description("系统")]
        System = 0,
        /// <summary>
        /// 模块
        /// </summary>
        [Description("模块")]
        Moudle = 1,
        /// <summary>
        /// 权限
        /// </summary>
        [Description("权限")]
        Permission = 2
    }
}
