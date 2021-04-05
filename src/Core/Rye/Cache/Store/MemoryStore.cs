using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rye.Cache.Store
{
    public class MemoryStore : IMemoryStore
    {
        private IMemoryCache _cache;

        public MemoryStore(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool ReadOnly { get => false; }

        public bool MultiCacheEnabled { get => false; }

        public bool Exist(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            return _cache.TryGetValue(key, out object _);
        }

        public Task<bool> ExistAsync(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            return Task.FromResult(_cache.TryGetValue(key, out object _));
        }

        public bool Exist(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            return _cache.TryGetValue(entry.Key, out object _);
        }

        public Task<bool> ExistAsync(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            return Task.FromResult(_cache.TryGetValue(entry.Key, out object _));
        }

        public void Set<T>(string key, T data, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            if (cacheSeconds == -1)
                _cache.Set(key, data);
            else
                _cache.Set(key, data, TimeSpan.FromSeconds(cacheSeconds));
        }

        public void Set<T>(CacheOptionEntry entry, T data)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            _cache.Set(entry.Key, data, entry.ConvertToMemoryCacheEntryOptions());
        }

        public Task SetAsync<T>(string key, T data, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            if (cacheSeconds == -1)
                _cache.Set(key, data);
            else
                _cache.Set(key, data, TimeSpan.FromSeconds(cacheSeconds));

            return Task.CompletedTask;
        }

        public Task SetAsync<T>(CacheOptionEntry entry, T data)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            _cache.Set(entry.Key, data, entry.ConvertToMemoryCacheEntryOptions());
            return Task.CompletedTask;
        }

        public T Get<T>(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            return _cache.Get<T>(key);
        }

        public T Get<T>(string key, Func<T> func, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            Check.NotNull(func, nameof(func));
            return _cache.GetOrCreate<T>(key, func, cacheSeconds);
        }
        
        public T Get<T>(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            return _cache.Get<T>(entry.Key);
        }

        public T Get<T>(CacheOptionEntry entry, Func<T> func)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            Check.NotNull(func, nameof(func));
            return _cache.GetOrCreate<T>(entry, func);
        }

        public Task<T> GetAsync<T>(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            return Task.FromResult(_cache.Get<T>(key));
        }

        public Task<T> GetAsync<T>(string key, Func<T> func, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            Check.NotNull(func, nameof(func));
            return Task.FromResult(_cache.GetOrCreate<T>(key, func, cacheSeconds));
        }

        public Task<T> GetAsync<T>(CacheOptionEntry entry, Func<T> func)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            Check.NotNull(func, nameof(func));
            return Task.FromResult(_cache.GetOrCreate<T>(entry, func));
        }

        public Task<T> GetAsync<T>(string key, Func<Task<T>> func, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            Check.NotNull(func, nameof(func));
            return _cache.GetOrCreateAsync<T>(key, func, cacheSeconds);
        }


        public Task<T> GetAsync<T>(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            return Task.FromResult(_cache.Get<T>(entry.Key));
        }

        public Task<T> GetAsync<T>(CacheOptionEntry entry, Func<Task<T>> func)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            Check.NotNull(func, nameof(func));
            return _cache.GetOrCreateAsync<T>(entry, func);
        }

        public void Remove(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            _cache.Remove(key);
        }

        public Task RemoveAsync(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            _cache.Remove(key);
            return Task.CompletedTask;
        }

        public void Remove(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            _cache.Remove(entry.Key);
        }

        public Task RemoveAsync(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            _cache.Remove(entry.Key);
            return Task.CompletedTask;
        }
    }
}