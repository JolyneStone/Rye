using System;
using System.Threading.Tasks;

namespace Rye.Cache.Store
{
    public abstract class AppCacheStore : IAppCacheStore
    {

        /// <summary>
        /// 根据缓存方案分配不同的CacheStore
        /// </summary>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public abstract ICacheStore AssignStore(CacheScheme scheme);

        public bool Exists(CacheScheme scheme, string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            var store = AssignStore(scheme);
            return store == null ? false : store.Exists(key);
        }

        public async Task<bool> ExistsAsync(CacheScheme scheme, string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            var store = AssignStore(scheme);
            return store == null ? false : await store.ExistsAsync(key);
        }

        public bool Exists(CacheScheme scheme, CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            var store = AssignStore(scheme);
            return store == null ? false : store.Exists(entry);
        }

        public async Task<bool> ExistsAsync(CacheScheme scheme, CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            var store = AssignStore(scheme);
            return store == null ? false : await store.ExistsAsync(entry);
        }

        public T Get<T>(CacheScheme scheme, string key)
        {
            var store = AssignStore(scheme);
            return store == null ? default : store.Get<T>(key);
        }

        public T Get<T>(CacheScheme scheme, string key, Func<T> func, int cacheSeconds = 60)
        {
            var store = AssignStore(scheme);
            return store == null ? default : store.Get<T>(key, func, cacheSeconds);
        }

        public T Get<T>(CacheScheme scheme, CacheOptionEntry entry)
        {
            var store = AssignStore(scheme);
            return store == null ? default : store.Get<T>(entry);
        }

        public T Get<T>(CacheScheme scheme, CacheOptionEntry entry, Func<T> func)
        {
            var store = AssignStore(scheme);
            return store == null ? default : store.Get<T>(entry, func);
        }

        public async Task<T> GetAsync<T>(CacheScheme scheme, string key)
        {
            var store = AssignStore(scheme);
            return store == null ? default : await store.GetAsync<T>(key);
        }

        public async Task<T> GetAsync<T>(CacheScheme scheme, string key, Func<T> func, int cacheSeconds = 60)
        {
            var store = AssignStore(scheme);
            return store == null ? default : await store.GetAsync<T>(key, func, cacheSeconds);
        }

        public async Task<T> GetAsync<T>(CacheScheme scheme, CacheOptionEntry entry)
        {
            var store = AssignStore(scheme);
            return store == null ? default : await store.GetAsync<T>(entry);
        }

        public async Task<T> GetAsync<T>(CacheScheme scheme, CacheOptionEntry entry, Func<T> func)
        {
            var store = AssignStore(scheme);
            return store == null ? default : await store.GetAsync<T>(entry, func);
        }

        public async Task<T> GetAsync<T>(CacheScheme scheme, string key, Func<Task<T>> func, int cacheSeconds = 60)
        {
            var store = AssignStore(scheme);
            return store == null ? default : await store.GetAsync<T>(key, func, cacheSeconds);
        }

        public async Task<T> GetAsync<T>(CacheScheme scheme, CacheOptionEntry entry, Func<Task<T>> func)
        {
            var store = AssignStore(scheme);
            return store == null ? default : await store.GetAsync<T>(entry, func);
        }

        public void Set<T>(CacheScheme scheme, string key, T data, int cacheSeconds = 60)
        {
            var store = AssignStore(scheme);
            if (store != null)
                store.Set<T>(key, data, cacheSeconds);
        }

        public void Set<T>(CacheScheme scheme, CacheOptionEntry entry, T data)
        {
            var store = AssignStore(scheme);
            if (store != null)
                store.Set<T>(entry, data);
        }

        public async Task SetAsync<T>(CacheScheme scheme, string key, T data, int cacheSeconds = 60)
        {
            var store = AssignStore(scheme);
            if (store != null)
                await store.SetAsync<T>(key, data, cacheSeconds);
        }

        public async Task SetAsync<T>(CacheScheme scheme, CacheOptionEntry entry, T data)
        {
            var store = AssignStore(scheme);
            if (store != null)
                await store.SetAsync<T>(entry, data);
        }


        public void Remove(CacheScheme scheme, string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            var store = AssignStore(scheme);
            if (store != null)
                store.Remove(key);
        }

        public async Task RemoveAsync(CacheScheme scheme, string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            var store = AssignStore(scheme);
            if (store != null)
                await store.RemoveAsync(key);
        }

        public void Remove(CacheScheme scheme, CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            var store = AssignStore(scheme);
            if (store != null)
                store.Remove(entry);
        }

        public async Task RemoveAsync(CacheScheme scheme, CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            var store = AssignStore(scheme);
            if (store != null)
                await store.RemoveAsync(entry);
        }
    }
}
