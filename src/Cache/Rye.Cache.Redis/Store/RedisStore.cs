using CSRedis;

using Rye.Cache.Redis.Internal;
using Rye.Cache.Redis.Options;
using Rye.Cache.Store;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rye.Cache.Redis.Store
{
    public class RedisStore : IRedisStore
    {
        private readonly CSRedisClient _redisClient;
        private readonly IMemoryStore _memoryStore;
        private readonly bool _readOnly;
        private readonly bool _multiCacheEnabled;
        private readonly string CLIENT_ID = Guid.NewGuid().ToString("N");
        private static readonly string SERVICE_ID = Guid.NewGuid().ToString("N");
        private static readonly string TOPIC_NAME = "Rye_Cache_Sub";
        public RedisStore(RedisOptions options)
        {
            _redisClient = new CSRedisClient(new RedisConnectionBuilder().BuildConnectionString(options), options.Sentinels, options.ReadOnly);
            _readOnly = options.ReadOnly;
            _multiCacheEnabled = options.MultiCacheEnabled;
            _memoryStore = null;
        }

        /// <summary>
        /// 若要使用多级缓存，请使用此构造器
        /// </summary>
        /// <param name="options"></param>
        /// <param name="memoryStore"></param>
        public RedisStore(RedisOptions options, IMemoryStore memoryStore)
        {
            _redisClient = new CSRedisClient(new RedisConnectionBuilder().BuildConnectionString(options), options.Sentinels, options.ReadOnly);
            _memoryStore = options.MultiCacheEnabled ? memoryStore : null;
            _readOnly = options.ReadOnly;
            _multiCacheEnabled = options.MultiCacheEnabled;
            if (_memoryStore != null)
            {
                _redisClient.SubscribeListBroadcast(TOPIC_NAME, CLIENT_ID, (msg) =>
                {
                    if (string.IsNullOrEmpty(msg))
                    {
                        return;
                    }

                    var message = msg.ToObject<CacheMessage>();
                    if (message == null)
                    {
                        return;
                    }

                    OnMessage(message);
                });
            }
        }

        #region 多级缓存

        private void OnMessage(CacheMessage message)
        {
            if (message == null ||
                _memoryStore == null ||
                message.Keys == null ||
                message.ServiceId == SERVICE_ID) //本程序发送的消息，不做处理
                return;

            //检测缓存是否存在
            for (var i = 0; i < message.Keys.Length; i++)
            {
                var key = message.Keys[i];
                if (string.IsNullOrEmpty(key))
                    continue;
                if (_memoryStore.Exist(key))
                {
                    _memoryStore.Remove(key);
                }
            }
        }

        /// <summary>
        /// 通知其他客户端清理过期缓存
        /// </summary>
        /// <param name="keys"></param>
        protected void Publish(string[] keys)
        {
            if (_memoryStore == null && keys == null)
                return;

            _redisClient.LPush(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = keys
            });
        }

        /// <summary>
        /// 通知其他客户端清理过期缓存
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        protected async Task PublishAsync(string[] keys)
        {
            if (_memoryStore == null && keys == null)
                return;

            await _redisClient.LPushAsync(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = keys
            });
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
            if (_memoryStore == null && string.IsNullOrEmpty(key) && Equals(data, default(T)))
                return;

            _memoryStore.Set(key, data, cacheSeconds);
            _redisClient.LPush(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = new string[] { key }
            });
        }

        /// <summary>
        /// 更新一级缓存，并通知其他客户端清理过期缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        /// <param name="data"></param>
        protected void UpdateAndPublish<T>(CacheOptionEntry entry, T data)
        {
            if (_memoryStore == null && entry == null && Equals(data, default(T)))
                return;

            _memoryStore.Set(entry, data);
            _redisClient.LPush(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = new string[] { entry.Key }
            });
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
            if (_memoryStore == null && string.IsNullOrEmpty(key) && Equals(data, default(T)))
                return;

            _memoryStore.Set(key, data, cacheSeconds);
            await _redisClient.LPushAsync(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = new string[] { key }
            });
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
            if (_memoryStore == null && entry == null && Equals(data, default(T)))
                return;

            _memoryStore.Set(entry, data);
            await _redisClient.LPushAsync(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = new string[] { entry.Key }
            });
        }

        /// <summary>
        /// 删除一级缓存，并通知其他客户端清理过期缓存
        /// </summary>
        /// <param name="key"></param>
        protected void RemoveAndPublish(string key)
        {
            if (_memoryStore == null && string.IsNullOrEmpty(key))
                return;

            _memoryStore.Remove(key);
            _redisClient.LPush(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = new string[] { key }
            });
        }

        /// <summary>
        /// 删除一级缓存，并通知其他客户端清理过期缓存
        /// </summary>
        /// <param name="entry"></param>
        protected void RemoveAndPublish(CacheOptionEntry entry)
        {
            if (_memoryStore == null && entry == null)
                return;

            _memoryStore.Remove(entry);
            _redisClient.LPush(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = new string[] { entry.Key }
            });
        }

        /// <summary>
        /// 删除一级缓存，并通知其他客户端清理过期缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected async Task RemoveAndPublishAsync(string key)
        {
            if (_memoryStore == null && string.IsNullOrEmpty(key))
                return;

            _memoryStore.Remove(key);
            await _redisClient.LPushAsync(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = new string[] { key }
            });
        }

        /// <summary>
        /// 删除一级缓存，并通知其他客户端清理过期缓存
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        protected async Task RemoveAndPublishAsync(CacheOptionEntry entry)
        {
            if (_memoryStore == null && entry == null)
                return;

            _memoryStore.Remove(entry);
            await _redisClient.LPushAsync(TOPIC_NAME, new CacheMessage
            {
                ServiceId = SERVICE_ID,
                Keys = new string[] { entry.Key }
            });
        }

        #endregion

        public bool ReadOnly { get => _readOnly; }

        public bool MultiCacheEnabled { get => _multiCacheEnabled; }

        public bool Exist(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            var exist = false;
            if (_memoryStore != null)
                exist = _memoryStore.Exist(key);

            return exist ? exist : _redisClient.Exists(key);
        }

        public bool Exist(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));

            var exist = false;
            if (_memoryStore != null)
                exist = _memoryStore.Exist(entry.Key);

            return exist ? exist : _redisClient.Exists(entry.Key);
        }

        public async Task<bool> ExistAsync(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            var exist = false;
            if (_memoryStore != null)
                exist = _memoryStore.Exist(key);

            return exist ? exist : await _redisClient.ExistsAsync(key);
        }

        public async Task<bool> ExistAsync(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            var exist = false;
            if (_memoryStore != null)
                exist = _memoryStore.Exist(entry.Key);

            return exist ? exist : await _redisClient.ExistsAsync(entry.Key);
        }
        public T Get<T>(string key, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            T data = default;
            if (_memoryStore != null)
                data = _memoryStore.Get<T>(key);

            if (!Equals(data, default))
                return data;

            data = _redisClient.Get<T>(key);
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
            T data = default;
            if (_memoryStore != null)
                data = _memoryStore.Get<T>(key);

            if (!Equals(data, default))
                return data;

            data = _redisClient.Get<T>(key);
            if (!Equals(data, default))
            {
                UpdateAndPublish(key, data,cacheSeconds);
                return data;
            }

            data = func();
            if (Equals(data, default))
                return default;

            _redisClient.Set(key, data, cacheSeconds);
            UpdateAndPublish(key, data, cacheSeconds);
            return data;
        }

        public T Get<T>(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            T data = default;
            if (_memoryStore != null)
                data = _memoryStore.Get<T>(entry.Key);

            if (!Equals(data, default))
                return data;

            data = _redisClient.Get<T>(entry.Key);
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
            T data = default;
            if (_memoryStore != null)
                data = _memoryStore.Get<T>(entry.Key);

            if (!Equals(data, default))
                return data;

            data = _redisClient.Get<T>(entry.Key);
            if (!Equals(data, default))
            {
                UpdateAndPublish(entry, data);
                return data;
            }

            data = func();
            if (Equals(data, default))
                return default;


            _redisClient.Set(entry.Key, data, entry.ConvertToTimeSpanRelativeToNow());
            UpdateAndPublish(entry, data);
            return data;
        }

        public async Task<T> GetAsync<T>(string key, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            T data = default;
            if (_memoryStore != null)
                data = _memoryStore.Get<T>(key);

            if (!Equals(data, default))
                return data;

            data = await _redisClient.GetAsync<T>(key);
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
            T data = default;
            if (_memoryStore != null)
                data = _memoryStore.Get<T>(key);

            if (!Equals(data, default))
                return data;

            data = await _redisClient.GetAsync<T>(key);
            if (!Equals(data, default))
            {
                await UpdateAndPublishAsync(key, data, cacheSeconds);
                return data;
            }

            data = func();
            if (Equals(data, default))
                return default;

            await _redisClient.SetAsync(key, data, cacheSeconds);
            await UpdateAndPublishAsync(key, data, cacheSeconds);
            return data;
        }

        public async Task<T> GetAsync<T>(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            T data = default;
            if (_memoryStore != null)
                data = _memoryStore.Get<T>(entry.Key);

            if (!Equals(data, default))
                return data;

            data = await _redisClient.GetAsync<T>(entry.Key);
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
            T data = default;
            if (_memoryStore != null)
                data = _memoryStore.Get<T>(entry.Key);

            if (!Equals(data, default))
                return data;

            data = await _redisClient.GetAsync<T>(entry.Key);
            if (!Equals(data, default))
            {
                await UpdateAndPublishAsync(entry, data);
                return data;
            }

            data = func();
            if (Equals(data, default))
                return default;

            await _redisClient.SetAsync(entry.Key, data, entry.ConvertToTimeSpanRelativeToNow());
            await UpdateAndPublishAsync(entry, data);
            return data;
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> func, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            Check.NotNull(func, nameof(func));
            T data = default;
            if (_memoryStore != null)
                data = _memoryStore.Get<T>(key);

            if (!Equals(data, default))
                return data;

            data = await _redisClient.GetAsync<T>(key);
            if (!Equals(data, default))
            {
                await UpdateAndPublishAsync(key, data, cacheSeconds);
                return data;
            }

            data = await func();
            if (Equals(data, default))
                return default;

            await _redisClient.SetAsync(key, data, cacheSeconds);
            await UpdateAndPublishAsync(key, data, cacheSeconds);
            return data;
        }

        public async Task<T> GetAsync<T>(CacheOptionEntry entry, Func<Task<T>> func)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            Check.NotNull(func, nameof(func));
            T data = default;
            if (_memoryStore != null)
                data = _memoryStore.Get<T>(entry.Key);

            if (!Equals(data, default))
                return data;

            data = await _redisClient.GetAsync<T>(entry.Key);
            if (!Equals(data, default))
            {
                await UpdateAndPublishAsync(entry, data);
                return data;
            }

            data = await func();
            if (Equals(data, default))
                return default;

            if (_memoryStore != null)
                _memoryStore.Set(entry, data);

            await _redisClient.SetAsync(entry.Key, data, entry.ConvertToTimeSpanRelativeToNow());
            await UpdateAndPublishAsync(entry, data);
            return data;
        }

        public void Remove(string key)
        {
            if (_readOnly) throw new NotSupportedException($"The {nameof(RedisStore)} is read only.");
            Check.NotNullOrEmpty(key, nameof(key));
            _redisClient.Del(key);
            RemoveAndPublish(key);
        }

        public void Remove(CacheOptionEntry entry)
        {
            if (_readOnly) throw new NotSupportedException($"The {nameof(RedisStore)} is read only.");
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            _redisClient.Del(entry.Key);
            RemoveAndPublish(entry.Key);
        }

        public async Task RemoveAsync(string key)
        {
            if (_readOnly) throw new NotSupportedException($"The {nameof(RedisStore)} is read only.");
            Check.NotNullOrEmpty(key, nameof(key));
            await _redisClient.DelAsync(key);
            await RemoveAndPublishAsync(key);
        }

        public async Task RemoveAsync(CacheOptionEntry entry)
        {
            if (_readOnly) throw new NotSupportedException($"The {nameof(RedisStore)} is read only.");
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            await _redisClient.DelAsync(entry.Key);
            await RemoveAndPublishAsync(entry.Key);
        }

        public void Set<T>(string key, T data, int cacheSeconds = 60)
        {
            if (_readOnly) throw new NotSupportedException($"The {nameof(RedisStore)} is read only.");
            Check.NotNullOrEmpty(key, nameof(key));
            _redisClient.Set(key, data, cacheSeconds);
            UpdateAndPublish(key, data, cacheSeconds);
        }

        public void Set<T>(CacheOptionEntry entry, T data)
        {
            if (_readOnly) throw new NotSupportedException($"The {nameof(RedisStore)} is read only.");
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            _redisClient.Set(entry.Key, data);
            UpdateAndPublish(entry, data);
        }

        public async Task SetAsync<T>(string key, T data, int cacheSeconds = 60)
        {
            if (_readOnly) throw new NotSupportedException($"The {nameof(RedisStore)} is read only.");
            Check.NotNullOrEmpty(key, nameof(key));
            await _redisClient.SetAsync(key, data, cacheSeconds);
            await UpdateAndPublishAsync(key, data, cacheSeconds);
        }

        public async Task SetAsync<T>(CacheOptionEntry entry, T data)
        {
            if (_readOnly) throw new NotSupportedException($"The {nameof(RedisStore)} is read only.");
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            await _redisClient.SetAsync(entry.Key, data, entry.ConvertToTimeSpanRelativeToNow());
            await UpdateAndPublishAsync(entry, data);
        }
    }
}
