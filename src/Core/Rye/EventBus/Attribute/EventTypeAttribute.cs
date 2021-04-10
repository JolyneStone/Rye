using System;

namespace Rye.EventBus
{
    /// <summary>
    /// 定义处理事件类型的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public class EventTypeAttribute: Attribute
    {
        public Type EventType { get; set; }

        public EventTypeAttribute(Type eventType)
        {
            EventType = eventType;
        }
    }
}
