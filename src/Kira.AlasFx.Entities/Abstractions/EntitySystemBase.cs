using Kira.AlasFx.Domain;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kira.AlasFx.Entities.Abstractions
{
    /// <summary>
    /// 系统实体基类
    /// </summary>
    /// <typeparam name="TSystemKey"></typeparam>
    public abstract class EntitySystemBase<TSystemKey> : EntityBase<TSystemKey> where TSystemKey : IEquatable<TSystemKey>
    {
        [NotMapped]
        public override TSystemKey Key { get => SystemId; }

        /// <summary>
        /// 系统主键
        /// </summary>
        public TSystemKey SystemId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
