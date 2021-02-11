using Monica.DataAccess;
using System;

namespace Monica.Entities.Abstractions
{
    public interface IEntityUserRoleBase<TUserKey, TRoleKey, TUserRoleKey> : IEntity<TUserRoleKey>
        where TUserKey : IEquatable<TUserKey>
        where TRoleKey:IEquatable<TRoleKey>
        where TUserRoleKey : IEquatable<TUserRoleKey>
    {
        /// <summary>
        /// 用户主键
        /// </summary>
        TUserKey UserId { get; set; }

        /// <summary>
        /// 角色主键
        /// </summary>
        TRoleKey RoleId { get; set; }
    }
}
