using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Raven.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IServiceProvider _service;
        private readonly IDistributedCache _cache;

        public CacheService(IServiceProvider service, IDistributedCache cache)
        {
            _service = service;
            _cache = cache;
        }

        public bool Exist(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            return _cache.Exist(key);
        }

        public Task<bool> ExistAsync(string key, CancellationToken token = default)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            return _cache.ExistAsync(key, token);
        }

        public T Get<T>(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            return _cache.Get<T>(key);
        }

        public T Get<T>(string key, Func<IServiceProvider, T> func, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            Check.NotNull(func, nameof(func));
            return _cache.Get<T>(key, () => func(_service), cacheSeconds);
        }

        public T Get<T>(string key, Func<IServiceProvider, T> func, DistributedCacheEntryOptions options)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            Check.NotNull(func, nameof(func));
            return _cache.Get<T>(key, () => func(_service), options);
        }

        public Task<T> GetAsync<T>(string key, CancellationToken token = default)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            return _cache.GetAsync<T>(key, token);
        }

        public Task<T> GetAsync<T>(string key, Func<IServiceProvider, T> func, int cacheSeconds = 60, CancellationToken token = default)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            Check.NotNull(func, nameof(func));
            return _cache.GetAsync<T>(key, () => func(_service), cacheSeconds,token);
        }

        public Task<T> GetAsync<T>(string key, Func<IServiceProvider, T> func, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            Check.NotNull(func, nameof(func));
            return _cache.GetAsync<T>(key, () => func(_service), options, token);
        }

        public Task<T> GetAsync<T>(string key, Func<IServiceProvider, Task<T>> func, int cacheSeconds = 60, CancellationToken token = default)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            Check.NotNull(func, nameof(func));
            return _cache.GetAsync<T>(key, async () => await func(_service), cacheSeconds, token);
        }

        public Task<T> GetAsync<T>(string key, Func<IServiceProvider, Task<T>> func, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            Check.NotNull(func, nameof(func));
            return _cache.GetAsync<T>(key, async () => await func(_service), options,token);
        }

        public void Refresh(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            _cache.Refresh(key);
        }

        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            return _cache.RefreshAsync(key);
        }

        public void Remove(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            _cache.Remove(key);
        }

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            return _cache.RemoveAsync(key, token);
        }
    }
}
