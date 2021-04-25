using CSRedis;

using Rye.Cache.Redis.Options;
using Rye.EventBus.Abstractions;

using System;
using System.Threading.Tasks;

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

        public Func<IEvent, RedisEventContext, Task> OnProducing { get; set; }

        public Func<IEvent, RedisEventContext, Task> OnProduced { get; set; }

        public Func<IEvent, RedisEventErrorContext, Task> OnProductError { get; set; }

        public Func<IEvent, RedisEventContext, Task> OnConsuming { get; set; }

        public Func<IEvent, RedisEventContext, Task> OnConsumed { get; set; }

        public Func<IEvent, RedisEventErrorContext, Task> OnConsumeError { get; set; }
    }
}
