using System;
using System.Data;
using System.Threading.Tasks;

namespace Rye.DataAccess
{
    /// <summary>
    /// 数据库连接者
    /// </summary>
    public class Connector : IDisposable
    {
        private volatile bool _used = false;
        /// <summary>
        /// 是否正在被使用中，只读
        /// </summary>
        public bool Used
        {
            get { return _used; }
            internal set { _used = value; }
        }

        private IDbConnection _conn = null;

        /// <summary>
        /// 数据库连接
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                if (_conn.State != ConnectionState.Open)
                    _conn.Open();
                return _conn;
            }
        }

        public Connector(IDbConnection dbConnection)
        {
            _conn = dbConnection;
        }

        /// <summary>
        /// 归还连接
        /// </summary>
        public void Return()
        {
            _used = false;
        }

        /// <summary>
        /// 这里不释放资源，只是归还连接
        /// </summary>
        public void Dispose()
        {
            this.Return();
        }


        #region static method

        /// <summary>
        /// 执行委托。注意：此方法会归还数据库连接者
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="action"></param>
        public static void Execute(Connector connector, Action<Connector> action)
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
        public static async Task ExecuteAsync(Connector connector, Func<Connector, Task> func)
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
        public static T Execute<T>(Connector connector, Func<Connector, T> func)
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
        public static async Task<T> ExecuteAsync<T>(Connector connector, Func<Connector, Task<T>> func)
        {
            using (connector)
            {
                return await func(connector);
            }
        }

        #endregion
    }
}
