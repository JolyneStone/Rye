using Rye.EventBus.Abstractions;

using System;
using System.Threading.Tasks;

namespace Rye.EventBus.Internal
{
    /// <summary>
    /// 内部消息处理程序，用来对非类型化的处理程序进行包装
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InternalEventHandler<TEvent, TContext> : EventHandler<TEvent, TContext>
        where TEvent: IEvent
        where TContext: EventContext
    {
        private readonly Func<TEvent, TContext, Task> _func;

        public InternalEventHandler(Func<TEvent, TContext, Task> func)
        {
            _func = func;
        }

        protected override Task OnEvent(TEvent @event, TContext eventContext)
        {
            return _func(@event, eventContext);
        }
    }
}
