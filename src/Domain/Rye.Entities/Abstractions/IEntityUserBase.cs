using System;

namespace Rye.Entities.Abstractions
{
    /// <summary>
    /// 用户实体基类
    /// </summary>
    /// <typeparam name="TUserKey"></typeparam>
    public interface IEntityUserBase<TUserKey> : IEntity<TUserKey> where TUserKey : IEquatable<TUserKey>
    {
        /// <summary>
        /// 用户主键
        /// </summary>
        TUserKey Id { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        sbyte Status { get; set; }
    }
}
