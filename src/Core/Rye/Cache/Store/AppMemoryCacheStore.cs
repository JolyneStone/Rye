namespace Rye.Cache.Store
{
    public class AppMemoryCacheStore : AppCacheStore
    {
        private IMemoryStore _memroyStore;

        public AppMemoryCacheStore(IMemoryStore memoryStore)
        {
            _memroyStore = memoryStore;
        }

        /// <summary>
        /// 根据缓存方案分配不同的CacheStore
        /// </summary>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public override ICacheStore AssignStore(CacheScheme scheme)
        {
            return scheme switch
            {
                CacheScheme.Memory or CacheScheme.MutilCache => _memroyStore,
                _ => null
            };
        }
    }
}
