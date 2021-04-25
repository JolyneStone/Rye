using System.Threading.Tasks;

namespace Rye.EventBus.Abstractions
{
    public interface IEventBus : IEventPublisher, IEventSubscriber
    {
        /// <summary>
        /// 重发事件
        /// </summary>
        /// <param name="event"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        Task RetryEvent(IEvent @event, EventContext context);
    }
}
