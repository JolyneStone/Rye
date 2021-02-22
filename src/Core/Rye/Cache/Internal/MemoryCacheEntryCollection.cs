namespace Rye.Cache.Internal
{
    public static class MemoryCacheEntryCollection
    {
        /// <summary>
        /// 获取多语言字典列表缓存Key
        /// </summary>
        /// <param name="expire"></param>
        /// <returns></returns>
        public static RyeMemoryCacheEntry GetLangDictionaryListEntry(int expire = 10 * 60)
        {
            return new RyeMemoryCacheEntry
            {
                CacheKey = "LangDictionary:GetEnableList",
                Expire = expire
            };
        }

        /// <summary>
        /// 获取AppInfo Sign 缓存Key
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public static RyeMemoryCacheEntry GetAppInfoSignEntry(string appId, int expire = 24 * 60 * 60)
        {
            return new RyeMemoryCacheEntry
            {
                CacheKey = $"AppInfo:Sign:{appId}",
                Expire = expire
            };
        }

        /// <summary>
        /// 获取AppInfo AppSecret 缓存Key
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public static RyeMemoryCacheEntry GetAppSecretEntry(string appKey, int expire = 24 * 60 * 60)
        {
            return new RyeMemoryCacheEntry
            {
                CacheKey = $"AppInfo:AppSecret:{appKey}",
                Expire = expire
            };
        }
    }

    public sealed class RyeMemoryCacheEntry
    {
        public string CacheKey { get; set; }
        public int Expire { get; set; }
    }
}
