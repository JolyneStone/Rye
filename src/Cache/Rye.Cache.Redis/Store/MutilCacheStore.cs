using Rye.Cache.Redis.Internal;
using Rye.Cache.Store;

using System;
using System.Threading.Tasks;

using static CSRedis.CSRedisClient;

namespace Rye.Cache.Redis.Store
{
    public class MutilCacheStore : IMutilCacheStore
    {
        private readonly IMemoryStore _memoryStore;
        private readonly IRedisStore _redisStore;
        private static readonly string SERVICE_ID = Guid.NewGuid().ToString("N");
        private static readonly string TOPIC_NAME = "RyeCacheSub";
        private readonly SubscribeObject _subscribeObject;
        public MutilCacheStore(IMemoryStore memoryStore, IRedisStore redisStore)
        {
            _memoryStore = memoryStore;
            _redisStore = redisStore;

            _subscribeObject = _redisStore.Client.Subscribe((TOPIC_NAME, (msg) =>
            {
                if (msg == null || string.IsNullOrEmpty(msg.Body))
                {
                    return;
                }

                var message = msg.Body.ToObject<CacheMessage>();
                if (message == null)
                {
                    return;
                }

                OnMessage(message);
            }
            ));
        }

        #region 多级缓存

        private void OnMessage(CacheMessage message)
        {
            if (message == null ||
                message.Keys == null ||
                message.ServiceId == SERVICE_ID) //非本程序发送的消息，不做处理
                return;

            //检测缓存是否存在
            for (var i = 0; i < message.Keys.Length; i++)
            {
                var key = message.Keys[i];
                if (string.IsNullOrEmpty(key))
                    continue;

                _memoryStore.Remove(key);
            }
        }

        /// <summary>
        /// 通知其他客户端清理过期缓存
        /// </summary>
        /// <param name="keys"></param>
        protected void Publish(string[] keys)
        {
            if (keys == null)
                return;

            _redisStore.Client.Publish(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = keys
            }.ToJsonString());
        }

        /// <summary>
        /// 通知其他客户端清理过期缓存
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        protected async Task PublishAsync(string[] keys)
        {
            if (keys == null)
                return;

            await _redisStore.Client.PublishAsync(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = keys
            }.ToJsonString());
        }

        /// <summary>
        /// 更新一级缓存，并通知其他客户端清理过期缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheSeconds"></param>
        protected void UpdateAndPublish<T>(string key, T data, int cacheSeconds)
        {
            if (string.IsNullOrEmpty(key) && Equals(data, default(T)))
                return;

            _memoryStore.Set(key, data, cacheSeconds);
            _redisStore.Client.Publish(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = new string[] { key }
            }.ToJsonString());
        }

        /// <summary>
        /// 更新一级缓存，并通知其他客户端清理过期缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        /// <param name="data"></param>
        protected void UpdateAndPublish<T>(CacheOptionEntry entry, T data)
        {
            if (entry == null && Equals(data, default(T)))
                return;

            _memoryStore.Set(entry, data);
            _redisStore.Client.Publish(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = new string[] { entry.Key }
            }.ToJsonString());
        }

        /// <summary>
        /// 更新一级缓存，并通知其他客户端清理过期缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheSeconds"></param>
        /// <returns></returns>
        protected async Task UpdateAndPublishAsync<T>(string key, T data, int cacheSeconds)
        {
            if (string.IsNullOrEmpty(key) && Equals(data, default(T)))
                return;

            _memoryStore.Set(key, data, cacheSeconds);
            await _redisStore.Client.PublishAsync(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = new string[] { key }
            }.ToJsonString());
        }

        /// <summary>
        /// 更新一级缓存，并通知其他客户端清理过期缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected async Task UpdateAndPublishAsync<T>(CacheOptionEntry entry, T data)
        {
            if (entry == null && Equals(data, default(T)))
                return;

            _memoryStore.Set(entry, data);
            await _redisStore.Client.PublishAsync(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = new string[] { entry.Key }
            }.ToJsonString());
        }

        /// <summary>
        /// 删除一级缓存，并通知其他客户端清理过期缓存
        /// </summary>
        /// <param name="key"></param>
        protected void RemoveAndPublish(string key)
        {
            if (string.IsNullOrEmpty(key))
                return;

            _memoryStore.Remove(key);
            _redisStore.Client.Publish(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = new string[] { key }
            }.ToJsonString());
        }

        /// <summary>
        /// 删除一级缓存，并通知其他客户端清理过期缓存
        /// </summary>
        /// <param name="entry"></param>
        protected void RemoveAndPublish(CacheOptionEntry entry)
        {
            if (entry == null)
                return;

            _memoryStore.Remove(entry);
            _redisStore.Client.Publish(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = new string[] { entry.Key }
            }.ToJsonString());
        }

        /// <summary>
        /// 删除一级缓存，并通知其他客户端清理过期缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected async Task RemoveAndPublishAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                return;

            _memoryStore.Remove(key);
            await _redisStore.Client.PublishAsync(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = new string[] { key }
            }.ToJsonString());
        }

        /// <summary>
        /// 删除一级缓存，并通知其他客户端清理过期缓存
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        protected async Task RemoveAndPublishAsync(CacheOptionEntry entry)
        {
            if (entry == null)
                return;

            _memoryStore.Remove(entry);
            await _redisStore.Client.PublishAsync(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = new string[] { entry.Key }
            }.ToJsonString());
        }

        #endregion

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _subscribeObject?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }

        public bool Exists(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));

            var exist = _memoryStore.Exists(key);
            return exist ? exist : _redisStore.Exists(key);
        }

        public bool Exists(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));

            var exist = _memoryStore.Exists(entry.Key);
            return exist ? exist : _redisStore.Exists(entry.Key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));

            var exist = _memoryStore.Exists(key);
            return exist ? exist : await _redisStore.ExistsAsync(key);
        }

        public async Task<bool> ExistsAsync(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));

            var exist = _memoryStore.Exists(entry.Key);
            return exist ? exist : await _redisStore.ExistsAsync(entry.Key);
        }

        public T Get<T>(string key)
        {
            return Get<T>(key, 60);
        }

        public Task<T> GetAsync<T>(string key)
        {
            return GetAsync<T>(key, 60);
        }

        public T Get<T>(string key, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));

            var data = _memoryStore.Get<T>(key);
            if (!Equals(data, default))
                return data;

            data = _redisStore.Get<T>(key);
            if (!Equals(data, default))
            {
                UpdateAndPublish(key, data, cacheSeconds);
            }

            return data;
        }
        public T Get<T>(string key, Func<T> func, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            Check.NotNull(func, nameof(func));

            T data = _memoryStore.Get<T>(key);

            if (!Equals(data, default))
                return data;

            data = _redisStore.Get<T>(key);
            if (!Equals(data, default))
            {
                UpdateAndPublish(key, data, cacheSeconds);
                return data;
            }

            data = func();
            if (Equals(data, default))
                return default;

            _redisStore.Set(key, data, cacheSeconds);
            UpdateAndPublish(key, data, cacheSeconds);
            return data;
        }

        public T Get<T>(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));

            var data = _memoryStore.Get<T>(entry.Key);
            if (!Equals(data, default))
                return data;

            data = _redisStore.Get<T>(entry.Key);
            if (!Equals(data, default))
            {
                UpdateAndPublish(entry, data);
            }

            return data;
        }

        public T Get<T>(CacheOptionEntry entry, Func<T> func)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            Check.NotNull(func, nameof(func));

            var data = _memoryStore.Get<T>(entry.Key);
            if (!Equals(data, default))
                return data;

            data = _redisStore.Get<T>(entry.Key);
            if (!Equals(data, default))
            {
                UpdateAndPublish(entry, data);
                return data;
            }

            data = func();
            if (Equals(data, default))
                return default;

            _redisStore.Set(entry, data);
            UpdateAndPublish(entry, data);
            return data;
        }

        public async Task<T> GetAsync<T>(string key, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));

            var data = _memoryStore.Get<T>(key);
            if (!Equals(data, default))
                return data;

            data = await _redisStore.GetAsync<T>(key);
            if (!Equals(data, default))
            {
                await UpdateAndPublishAsync(key, data, cacheSeconds);
            }

            return data;
        }

        public async Task<T> GetAsync<T>(string key, Func<T> func, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            Check.NotNull(func, nameof(func));

            var data = _memoryStore.Get<T>(key);
            if (!Equals(data, default))
                return data;

            data = await _redisStore.GetAsync<T>(key);
            if (!Equals(data, default))
            {
                await UpdateAndPublishAsync(key, data, cacheSeconds);
                return data;
            }

            data = func();
            if (Equals(data, default))
                return default;

            await _redisStore.SetAsync(key, data, cacheSeconds);
            await UpdateAndPublishAsync(key, data, cacheSeconds);
            return data;
        }

        public async Task<T> GetAsync<T>(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            var data = _memoryStore.Get<T>(entry.Key);

            if (!Equals(data, default))
                return data;

            data = await _redisStore.GetAsync<T>(entry.Key);
            if (!Equals(data, default))
            {
                await UpdateAndPublishAsync(entry, data);
            }

            return data;
        }

        public async Task<T> GetAsync<T>(CacheOptionEntry entry, Func<T> func)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            Check.NotNull(func, nameof(func));
            var data = _memoryStore.Get<T>(entry.Key);

            if (!Equals(data, default))
                return data;

            data = await _redisStore.GetAsync<T>(entry.Key);
            if (!Equals(data, default))
            {
                await UpdateAndPublishAsync(entry, data);
                return data;
            }

            data = func();
            if (Equals(data, default))
                return default;

            await _redisStore.SetAsync(entry, data);
            await UpdateAndPublishAsync(entry, data);
            return data;
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> func, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            Check.NotNull(func, nameof(func));

            var data = _memoryStore.Get<T>(key);
            if (!Equals(data, default))
                return data;

            data = await _redisStore.GetAsync<T>(key);
            if (!Equals(data, default))
            {
                await UpdateAndPublishAsync(key, data, cacheSeconds);
                return data;
            }

            data = await func();
            if (Equals(data, default))
                return default;

            await _redisStore.SetAsync(key, data, cacheSeconds);
            await UpdateAndPublishAsync(key, data, cacheSeconds);
            return data;
        }

        public async Task<T> GetAsync<T>(CacheOptionEntry entry, Func<Task<T>> func)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            Check.NotNull(func, nameof(func));

            var data = _memoryStore.Get<T>(entry.Key);
            if (!Equals(data, default))
                return data;

            data = await _redisStore.GetAsync<T>(entry.Key);
            if (!Equals(data, default))
            {
                await UpdateAndPublishAsync(entry, data);
                return data;
            }

            data = await func();
            if (Equals(data, default))
                return default;

            _memoryStore.Set(entry, data);

            await _redisStore.SetAsync(entry, data);
            await UpdateAndPublishAsync(entry, data);
            return data;
        }

        public void Remove(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            _redisStore.Remove(key);
            RemoveAndPublish(key);
        }

        public void Remove(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            _redisStore.Remove(entry.Key);
            RemoveAndPublish(entry.Key);
        }

        public async Task RemoveAsync(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            await _redisStore.RemoveAsync(key);
            await RemoveAndPublishAsync(key);
        }

        public async Task RemoveAsync(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            await _redisStore.RemoveAsync(entry.Key);
            await RemoveAndPublishAsync(entry.Key);
        }

        public void Set<T>(string key, T data, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            _redisStore.Set(key, data, cacheSeconds);
            UpdateAndPublish(key, data, cacheSeconds);
        }

        public void Set<T>(CacheOptionEntry entry, T data)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            _redisStore.Set(entry.Key, data);
            UpdateAndPublish(entry, data);
        }

        public async Task SetAsync<T>(string key, T data, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            await _redisStore.SetAsync(key, data, cacheSeconds);
            await UpdateAndPublishAsync(key, data, cacheSeconds);
        }

        public async Task SetAsync<T>(CacheOptionEntry entry, T data)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            await _redisStore.SetAsync(entry, data);
            await UpdateAndPublishAsync(entry, data);
        }
    }
}
