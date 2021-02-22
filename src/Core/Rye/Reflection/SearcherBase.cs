using System;
using System.Collections.Generic;
using System.Linq;

namespace Rye.Reflection
{
    public abstract class SearcherBase<T> : ISearcher<T>
    {
        private readonly object _lockObj = new object();

        /// <summary>
        /// 项缓存
        /// </summary>
        private readonly List<T> ItemsCache = new List<T>();

        /// <summary>
        /// 查找所有项
        /// </summary>
        /// <returns></returns>
        protected abstract List<T> FindAllItems();

        public List<T> Get(Func<T, bool> predicate, bool fromCache = false)
        {
            return GetAll(fromCache).Where(predicate).ToList();
        }

        public List<T> GetAll(bool fromCache = false)
        {
            if (fromCache && ItemsCache.Count > 0)
            {
                return ItemsCache;
            }
            else
            {
                lock (_lockObj)
                {
                    var items = FindAllItems();
                    ItemsCache.Clear();
                    ItemsCache.AddRange(items);
                    return items;
                }
            }
        }
    }
}
