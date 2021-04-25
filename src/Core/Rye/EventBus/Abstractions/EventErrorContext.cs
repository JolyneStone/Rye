using System;

namespace Rye.EventBus.Abstractions
{
    public abstract class EventErrorContext: EventContext
    {
        public Exception Exception { get; init; }

        public virtual void Throw()
        {
            throw Exception;
        }
    }
}
