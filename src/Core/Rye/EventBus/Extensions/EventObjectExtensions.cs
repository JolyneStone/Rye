using Rye.EventBus.Abstractions;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rye.EventBus
{
    public static class EventObjectExtensions
    {
        private static readonly Dictionary<Type, string> _eventRouteDict = new Dictionary<Type, string>();
        private static readonly Dictionary<Type, Type> _eventTypeDict = new Dictionary<Type, Type>();
        private static readonly Type _eventType = typeof(IEvent);
        private static readonly object _routeLocker = new object();
        private static readonly object _typeLocker = new object();

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
            lock (_routeLocker)
            {
                _eventRouteDict[type] = route;
            }
            return route;
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

            var attribute = type.GetCustomAttribute<EventTypeAttribute>(true);
            if (attribute != null)
            {
                Type eventType = attribute.EventType;
                lock (_typeLocker)
                {
                    _eventTypeDict[type] = eventType;
                }

                return eventType;
            }
            else
            {
                var baseType = type.BaseType;
                while (baseType != null)
                {
                    if (baseType.IsGenericType)
                    {
                        var defineType = baseType.GetGenericArguments()[0];
                        if (_eventType.IsAssignableFrom(defineType))
                        {
                            return defineType;
                        }
                    }

                    baseType = baseType.BaseType;
                }

                var interfaceTypes = type.GetInterfaces();
                if (interfaceTypes == null)
                    return null;

                foreach (var interfaceType in interfaceTypes)
                {
                    if (interfaceType.IsGenericType)
                    {
                        var defineType = interfaceType.GetGenericArguments()[0];
                        if (_eventType.IsAssignableFrom(defineType))
                        {
                            return defineType;
                        }
                    }
                }

                return null;
            }
        }
    }
}
