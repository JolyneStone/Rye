using System;

namespace Rye.Entities.Abstractions
{
    /// <summary>
    /// 角色实体基类
    /// </summary>
    /// <typeparam name="TRoleKey"></typeparam>
    public interface IEntityRoleBase<TRoleKey, TAppKey>: IEntity<TRoleKey> 
        where TRoleKey: IEquatable<TRoleKey>
        where TAppKey: IEquatable<TAppKey>
    {
        /// <summary>
        /// 角色主键
        /// </summary>
        TRoleKey Id { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        sbyte Status { get; set; }

        /// <summary>
        /// App Id
        /// </summary>
        TAppKey AppId { get; set; }
    }
}
