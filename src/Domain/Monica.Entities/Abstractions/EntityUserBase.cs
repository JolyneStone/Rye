using Monica.DataAccess;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monica.Entities.Abstractions
{
    /// <summary>
    /// 用户实体基类
    /// </summary>
    /// <typeparam name="TUserKey"></typeparam>
    public abstract class EntityUserBase<TUserKey> : EntityBase<TUserKey> where TUserKey: IEquatable<TUserKey>
    {
        [NotMapped]
        public override TUserKey Key { get => UserId; }

        /// <summary>
        /// 用户主键
        /// </summary>
        public TUserKey UserId { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// 实体状态
        /// </summary>
        public EntityStatus EntityStatus { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
