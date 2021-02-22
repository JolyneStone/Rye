using System;

namespace Rye.Entities.Abstractions
{
    /// <summary>
    /// 角色权限实体基类
    /// </summary>
    /// <typeparam name="TRoleKey"></typeparam>
    /// <typeparam name="TPermissionKey"></typeparam>
    /// <typeparam name="TRolePermissionKey"></typeparam>
    public interface IEntityRolePermissionBase<TRoleKey, TPermissionKey, TRolePermissionKey> : IEntity<TRolePermissionKey>
        where TRoleKey: IEquatable<TRoleKey>
        where TPermissionKey: IEquatable<TPermissionKey>
        where TRolePermissionKey : IEquatable<TRolePermissionKey>
    {
        /// <summary>
        /// 角色主键
        /// </summary>
        TRoleKey RoleId { get; set; }

        /// <summary>
        /// 权限主键
        /// </summary>
        TPermissionKey PermissionId { get; set; }
    }
}
