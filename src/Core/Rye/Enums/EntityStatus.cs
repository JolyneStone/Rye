using System.ComponentModel;

namespace Rye.Enums
{
    /// <summary>
    /// 实体状态枚举
    /// </summary>
    public enum EntityStatus: byte
    {
        /// <summary>
        /// 禁用
        /// </summary>
        [Description("禁用")]
        Disabled = 0,
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enabled = 1,
        /// <summary>
        /// 已删除
        /// </summary>
        [Description("已删除")]
        Deleted = 2
    }
}
