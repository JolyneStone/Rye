using Rye.DependencyInjection;

using System;

namespace Rye.EventBus
{
    /// <summary>
    /// 定义RabbitMQ事件路由的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public class RabbitMQEventRouteAttribute: Attribute
    {
        /// <summary>
        /// 路由键
        /// </summary>
        public string RouteKey { get; set; }

        /// <summary>
        /// 交换机
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 队列
        /// </summary>
        public string Queue { get; set; }

        public RabbitMQEventRouteAttribute(string routeKey)
        {
            Check.NotNull(routeKey, nameof(routeKey));
            RouteKey = routeKey;
        }
    }
}
