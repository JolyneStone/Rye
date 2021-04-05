using Rye.Cache;

using System;

namespace Demo.Core.Common
{
    public sealed class CacheEntrys
    {
        public static CacheOptionEntry LangDictionaryList()
        {
            return new CacheOptionEntry
            {
                Key = "LangDictionary:GetEnableList",
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10 * 60)
            };
        }

        public static CacheOptionEntry AppInfoById(int appId)
        {
            return new CacheOptionEntry
            {
                Key = string.Format("AppInfo:Id:{0}", appId),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(24 * 60 * 60)
            };
        }

        public static CacheOptionEntry AppInfoByKey(string appKey)
        {
            return new CacheOptionEntry
            {
                Key = string.Format("AppInfo:Key:{0}", appKey),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(24 * 60 * 60)
            };
        }
    }
}
