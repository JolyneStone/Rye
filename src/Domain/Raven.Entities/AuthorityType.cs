using System.ComponentModel;

namespace Raven.Entities
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
