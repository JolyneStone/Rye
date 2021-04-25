using Rye.EventBus.Abstractions;
using Rye.Util;

namespace Rye.Test.EventBus
{
    public class TestEvent : IEvent
    {
        public TestEvent()
        {
            EventId = IdGenerator.Instance.NextId();
        }

        public long EventId { get; set; }
        public int Id { get; set; }
    }
}
