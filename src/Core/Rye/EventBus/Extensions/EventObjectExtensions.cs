using Rye.EventBus.Abstractions;
using Rye.Threading;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rye.EventBus
{
    public static class EventObjectExtensions
    {
        private static readonly Dictionary<Type, string> _eventRouteDict = new Dictionary<Type, string>();
        private static readonly Dictionary<Type, Type> _eventTypeDict = new Dictionary<Type, Type>();
        private static readonly LockObject _routeLocker = new LockObject();
        private static readonly LockObject _typeLocker = new LockObject();

        /// <summary>
        /// 获取事件路由
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public static string GetEventRoute(this IEvent @event)
        {
            if (@event == null)
                return null;

            return GetEventRoute(@event.GetType());
        }

        internal static string GetEventRoute(this Type type)
        {
            if (type == null)
                return null;

            if (_eventRouteDict.ContainsKey(type))
            {
                return _eventRouteDict[type];
            }
            _routeLocker.Enter();
            try
            {
                string route;
                var attribute = type.GetCustomAttribute<EventRouteAttribute>(true);
                if (attribute != null)
                {
                    route = attribute.Route;
                }
                else
                {
                    route = type.Name;
                    if (route.EndsWith("Event", StringComparison.InvariantCultureIgnoreCase))
                        route = route.Substring(0, route.Length - 5);
                }
                _eventRouteDict[type] = route;
                return route;
            }
            finally
            {
                _routeLocker.Exit();
            }
        }

        /// <summary>
        /// 获取处理的事件类型
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static Type GetEventType(this IEventHandler handler)
        {
            if (handler == null)
                return null;

            var type = handler.GetType();
            if (_eventTypeDict.ContainsKey(type))
            {
                return _eventTypeDict[type];
            }
            _typeLocker.Enter();
            try
            {
                Type eventType = null;
                var attribute = type.GetCustomAttribute<EventTypeAttribute>(true);
                eventType = attribute?.EventType;
                _eventTypeDict[type] = eventType;
                return eventType;
            }
            finally
            {
                _typeLocker.Exit();
            }
        }
    }
}
