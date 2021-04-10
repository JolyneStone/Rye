using Rye.EventBus.Abstractions;

namespace Rye.EventBus.Redis
{
    public abstract class RedisEventHandler<TEvent>: EventHandler<TEvent, RedisEventContext>
        where TEvent: IEvent
    {
    }
}
