using Monica.DataAccess;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monica.Entities.Abstractions
{
    /// <summary>
    /// 角色实体基类
    /// </summary>
    /// <typeparam name="TRoleKey"></typeparam>
    public abstract class EntityRoleBase<TRoleKey>: EntityBase<TRoleKey> where TRoleKey: IEquatable<TRoleKey>
    {
        [NotMapped]
        public override TRoleKey Key { get => RoleId; }

        /// <summary>
        /// 角色主键
        /// </summary>
        public TRoleKey RoleId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
