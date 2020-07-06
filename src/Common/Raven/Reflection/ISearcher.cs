using System;
using System.Collections.Generic;

namespace Raven.Reflection
{
    public interface ISearcher<T>
    {
        /// <summary>
        /// 查找指定条件的项
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <param name="fromCache">是否来自缓存</param>
        /// <returns></returns>
        List<T> Get(Func<T, bool> predicate, bool fromCache = false);

        /// <summary>
        /// 查找所有项
        /// </summary>
        /// <param name="fromCache">是否来自缓存</param>
        /// <returns></returns>
        List<T> GetAll(bool fromCache = false);
    }
}
