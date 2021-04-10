using CSRedis;

using Rye.Cache.Redis.Options;

using System;

namespace Rye.EventBus.Redis.Options
{
    public class RedisEventBusOptions
    {
        /// <summary>
        /// 订阅Key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 客户端标识
        /// </summary>
        public string ClientId { get; set; }
        public Action<RedisOptions> RedisOptions { get; set; }
        public CSRedisClient RedisClient { get; set; }

    }
}
