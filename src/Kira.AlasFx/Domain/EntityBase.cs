using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.AlasFx.Domain
{
    /// <summary>
    /// 实体类基类
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class EntityBase<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        public virtual TKey Key { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is EntityBase<TKey> entity))
            {
                return false;
            }
            return IsKeyEqual(entity.Key, Key);
        }

        /// <summary>
        /// 实体Key是否相等
        /// </summary>
        public static bool IsKeyEqual(TKey id1, TKey id2)
        {
            if (id1 == null && id2 == null)
            {
                return true;
            }
            if (id1 == null || id2 == null)
            {
                return false;
            }

            return id1.Equals(id2);
        }

        public override int GetHashCode()
        {
            var key = Key;
            return key == null ? default : key.GetHashCode();
        }
    }
}
