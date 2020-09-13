using Monica.DataAccess;
using System;

namespace Monica.Entities.Abstractions
{
    /// <summary>
    /// 角色权限实体基类
    /// </summary>
    /// <typeparam name="TRoleKey"></typeparam>
    /// <typeparam name="TAuthorityKey"></typeparam>
    /// <typeparam name="TRoleAuthorityKey"></typeparam>
    public abstract class EntityRoleAuthorityBase<TRoleKey, TAuthorityKey, TRoleAuthorityKey>: EntityBase<TRoleAuthorityKey>
        where TRoleKey: IEquatable<TRoleKey>
        where TAuthorityKey: IEquatable<TAuthorityKey>
        where TRoleAuthorityKey: IEquatable<TRoleAuthorityKey>
    {
        /// <summary>
        /// 角色主键
        /// </summary>
        public TRoleKey RoleId { get; set; }

        /// <summary>
        /// 权限主键
        /// </summary>
        public TAuthorityKey AuthorityId { get; set; }

        public virtual EntityRoleBase<TRoleKey> Role { get; set; }

        public virtual EntityAuthorityBase<TAuthorityKey> Authority { get; set; }
    }
}
