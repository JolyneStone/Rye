using Monica.DataAccess;
using System;

namespace Monica.Entities.Abstractions
{
    public abstract class EntityUserRoleBase<TUserKey, TRoleKey, TUserRoleKey> : EntityBase<TUserRoleKey>
        where TUserKey : IEquatable<TUserKey>
        where TRoleKey:IEquatable<TRoleKey>
        where TUserRoleKey : IEquatable<TUserRoleKey>
    {
        /// <summary>
        /// 用户主键
        /// </summary>
        public TUserKey UserId { get; set; }

        /// <summary>
        /// 角色主键
        /// </summary>
        public TRoleKey RoleId { get; set; }

        public virtual EntityUserBase<TUserKey> User { get; set; }

        public virtual EntityRoleBase<TRoleKey> Role { get; set; }
    }
}
