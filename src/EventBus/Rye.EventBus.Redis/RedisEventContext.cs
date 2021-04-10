using Rye.EventBus.Abstractions;

using System;

namespace Rye.EventBus.Redis
{
    public class RedisEventContext: EventContext
    {
        public string Key { get; init; }
    }
}
