using Monica.DataAccess;
using System;

namespace Monica.Entities.Abstractions
{
    /// <summary>
    /// 角色实体基类
    /// </summary>
    /// <typeparam name="TRoleKey"></typeparam>
    public interface IEntityRoleBase<TRoleKey>: IEntity<TRoleKey> where TRoleKey: IEquatable<TRoleKey>
    {
        /// <summary>
        /// 角色主键
        /// </summary>
        TRoleKey Id { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        sbyte Status { get; set; }
    }
}
