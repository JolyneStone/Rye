namespace Rye.Cache.Redis.Internal
{
    internal class CacheMessage
    {
        public string ServiceId { get; set; }
        public string[] Keys { get; set; }
    }
}
