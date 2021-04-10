namespace Rye.Enums
{
    /// <summary>
    /// 缓存存储方式
    /// </summary>
    public enum CacheStoreType
    {
        /// <summary>
        /// 使用MemoryCache存储
        /// </summary>
        Memory = 1,
        /// <summary>
        /// 使用Redis存储
        /// </summary>
        Redis = 2
    }
}
