using Rye.Cache.Store;

namespace Rye.Cache.Redis.Store
{
    public class AppRedisCacheStore : AppCacheStore
    {
        private readonly IMemoryStore _memoryStore;
        private readonly IRedisStore _redisStore;
        private readonly IMutilCacheStore _mutilCacheStore;

        public AppRedisCacheStore(
            IMemoryStore memoryStore,
            IRedisStore redisStore,
            IMutilCacheStore mutilCacheStore)
        {
            _memoryStore = memoryStore;
            _redisStore = redisStore;
            _mutilCacheStore = mutilCacheStore;
        }

        public override ICacheStore AssignStore(CacheScheme scheme)
        {
            return scheme switch
            {
                CacheScheme.Memory => _memoryStore,
                CacheScheme.Redis => _redisStore,
                CacheScheme.MutilCache => _mutilCacheStore,
                _ => null,
            };
        }
    }
}
