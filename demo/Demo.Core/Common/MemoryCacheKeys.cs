namespace Demo.Core.Common
{
    public sealed class MemoryCacheKeys
    {
        public const string LangDictionaryList = "LangDictionary:GetEnableList";
        public const int LangDictionaryList_TimeOut = 10 * 60;

        public const string AppInfoById = "AppInfo:Id:{0}";
        public const int AppInfoById_TimeOut = 24 * 60 * 60;

        public const string AppInfoByKey = "AppInfo:Key:{0}";
        public const int AppInfoByKey_TimeOut = 24 * 60 * 60;
    }
}
