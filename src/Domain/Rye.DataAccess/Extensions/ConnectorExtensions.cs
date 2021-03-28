using Rye.DataAccess.Pool;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rye.DataAccess
{
    public static class ConnectorExtensions
    {
        /// <summary>
        /// 执行委托。注意：此方法会归还数据库连接者
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="action"></param>
        public static void Execute(this Connector connector, Action<Connector> action)
        {
            using (connector)
            {
                action(connector);
            }
        }

        /// <summary>
        /// 执行委托。注意：此方法会归还数据库连接者
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task ExecuteAsync(this Connector connector, Func<Connector, Task> func)
        {
            using (connector)
            {
                await func(connector);
            }
        }

        /// <summary>
        /// 执行委托，返回委托结果。注意：此方法会归还数据库连接者
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connector"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T Execute<T>(this Connector connector, Func<Connector, T> func)
        {
            using (connector)
            {
                return func(connector);
            }
        }

        /// <summary>
        /// 执行委托，返回委托结果。注意：此方法会归还数据库连接者
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connector"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task<T> ExecuteAsync<T>(this Connector connector, Func<Connector, Task<T>> func)
        {
            using (connector)
            {
                return await func(connector);
            }
        }
    }
}
