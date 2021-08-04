namespace Rye
{
    /// <summary>
    /// 缓存存储方式
    /// </summary>
    public enum CacheScheme
    {
        /// <summary>
        /// 禁用缓存
        /// </summary>
        None = 0,
        /// <summary>
        /// 使用MemoryCache存储
        /// </summary>
        Memory = 1,
        /// <summary>
        /// 使用Redis存储
        /// </summary>
        Redis = 2,
        /// <summary>
        /// 使用多级缓存
        /// </summary>
        MutilCache = 3
    }
}
