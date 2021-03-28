using System;

namespace Rye.Entities.Abstractions
{
    /// <summary>
    /// 权限实体基类
    /// </summary>
    /// <typeparam name="TPermissionKey"></typeparam>
    public interface IEntityPermissionBase<TPermissionKey> : IEntity<TPermissionKey> where TPermissionKey : IEquatable<TPermissionKey>
    {
        /// <summary>
        /// 权限主键
        /// </summary>
        TPermissionKey Id { get; set; }

        /// <summary>
        /// 父级主键
        /// </summary>
        TPermissionKey ParentId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        sbyte Status { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        string Code { get; set; }

        /// <summary>
        /// 权限类型
        /// </summary>
        sbyte Type { get; set; }
    }
}
