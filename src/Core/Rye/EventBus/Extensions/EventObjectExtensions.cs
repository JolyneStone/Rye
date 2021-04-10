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
        private static readonly LockObject _locker = new LockObject();

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
            _locker.Enter();
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
            catch (Exception)
            {
                return "枚举错误";
            }
            finally
            {
                _locker.Exit();
            }
        }
    }
}
