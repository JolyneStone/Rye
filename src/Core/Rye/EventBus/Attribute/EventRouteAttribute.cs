using System;

namespace Rye.EventBus
{
    /// <summary>
    /// 定义事件路由的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public class EventRouteAttribute: Attribute
    {
        /// <summary>
        /// 路由
        /// </summary>
        public string Route { get; set; }

        public EventRouteAttribute(string route)
        {
            Check.NotNull(route, nameof(route));
            Route = route;
        }
    }
}
