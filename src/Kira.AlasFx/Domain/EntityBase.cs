using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kira.AlasFx.Domain
{
    /// <summary>
    /// 实体基类
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class EntityBase<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        [NotMapped]
        public abstract TKey Key { get; }

        /// <summary>
        /// 判断实体是否相同
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }
            if(!(obj is EntityBase<TKey> other))
            {
                return false;
            }

            var key1 = Key;
            var key2 = other.Key;
            if(key1 == null || key2 == null)
            {
                return false;
            }

            return key1.Equals(key2);
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var key = Key;
            return key == null ? 0 : key.GetHashCode();
        }
    }
}
