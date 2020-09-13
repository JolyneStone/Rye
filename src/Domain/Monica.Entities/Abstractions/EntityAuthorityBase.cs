using Monica.DataAccess;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monica.Entities.Abstractions
{
    /// <summary>
    /// 权限实体基类
    /// </summary>
    /// <typeparam name="TAuthorityKey"></typeparam>
    public abstract class EntityAuthorityBase<TAuthorityKey> : EntityBase<TAuthorityKey> where TAuthorityKey : IEquatable<TAuthorityKey>
    {
        [NotMapped]
        public override TAuthorityKey Key { get => Id; }

        /// <summary>
        /// 权限主键
        /// </summary>
        public TAuthorityKey Id { get; set; }

        /// <summary>
        /// 父级主键
        /// </summary>
        public TAuthorityKey ParentId { get; set; }

        /// <summary>
        /// 权限类型
        /// </summary>
        public AuthorityType Type { get; set; }
    }
}
