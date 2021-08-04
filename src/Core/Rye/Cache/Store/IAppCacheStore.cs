using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rye.Cache.Store
{
    public interface IAppCacheStore
    {
        /// <summary>
        /// 根据缓存方案分配不同的CacheStore
        /// </summary>
        /// <param name="scheme"></param>
        /// <returns></returns>
        ICacheStore AssignStore(CacheScheme scheme);

        bool Exists(CacheScheme scheme, string key);

        Task<bool> ExistsAsync(CacheScheme scheme, string key);

        bool Exists(CacheScheme scheme, CacheOptionEntry entry);

        Task<bool> ExistsAsync(CacheScheme scheme, CacheOptionEntry entry);

        T Get<T>(CacheScheme scheme, string key);

        T Get<T>(CacheScheme scheme, string key, Func<T> func, int cacheSeconds = 60);

        T Get<T>(CacheScheme scheme, CacheOptionEntry entry);

        T Get<T>(CacheScheme scheme, CacheOptionEntry entry, Func<T> func);

        Task<T> GetAsync<T>(CacheScheme scheme, string key);

        Task<T> GetAsync<T>(CacheScheme scheme, string key, Func<T> func, int cacheSeconds = 60);

        Task<T> GetAsync<T>(CacheScheme scheme, CacheOptionEntry entry);

        Task<T> GetAsync<T>(CacheScheme scheme, CacheOptionEntry entry, Func<T> func);

        Task<T> GetAsync<T>(CacheScheme scheme, string key, Func<Task<T>> func, int cacheSeconds = 60);

        Task<T> GetAsync<T>(CacheScheme scheme, CacheOptionEntry entry, Func<Task<T>> func);

        void Set<T>(CacheScheme scheme, string key, T data, int cacheSeconds = 60);

        void Set<T>(CacheScheme scheme, CacheOptionEntry entry, T data);

        Task SetAsync<T>(CacheScheme scheme, string key, T data, int cacheSeconds = 60);

        Task SetAsync<T>(CacheScheme scheme, CacheOptionEntry entry, T data);

        void Remove(CacheScheme scheme, string key);

        Task RemoveAsync(CacheScheme scheme, string key);

        void Remove(CacheScheme scheme, CacheOptionEntry entry);

        Task RemoveAsync(CacheScheme scheme, CacheOptionEntry entry);
    }
}
