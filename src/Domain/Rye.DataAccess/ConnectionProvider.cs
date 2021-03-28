using Microsoft.Extensions.Options;

using Rye.Configuration;
using Rye.DataAccess.Options;
using Rye.DataAccess.Pool;
using Rye.Threading;

using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;

namespace Rye.DataAccess
{
    public abstract class ConnectionProvider : IConnectionProvider
    {
        private readonly LockObject _locker = new LockObject();
        //private readonly Dictionary<string, IDbConnection> _connnectionPools = new Dictionary<string, IDbConnection>();
        private readonly Dictionary<string, ConnectionPool> _connectionPools = new Dictionary<string, ConnectionPool>();
        private readonly DbConnectionMapOptions _options;

        public ConnectionProvider(IOptions<DbConnectionMapOptions> options)
        {
            _options = options.Value;
        }

        public Connector GetConnection(bool usePool = true)
        {
            string connectionString = GetWriteDbConnectionString();
            return GetDbConnection(connectionString, usePool);
        }

        public Connector GetReadOnlyConnection(bool usePool = true)
        {
            string connectionString = GetRealOnlyDbConnectionString();
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = GetWriteDbConnectionString();
            }

            return GetDbConnection(connectionString, usePool);
        }

        public virtual string GetConnectionString(string connectionName)
        {
            //return ConfigurationManager.GetSectionValue($"Framework:DbConnections:{connectionName}:ConnectionString");
            return _options[connectionName]?.ConnectionString;
        }

        public Connector GetDbConnection(string connectionString, bool usePool = true)
        {
            Connector conn;
            if (!usePool)
            {
                conn = new Connector(GetDbConnectionCore(connectionString));
            }
            else
            {
                conn = GetDbConnectionByPool(connectionString);
                if (conn == null)
                {
                    conn = new Connector(GetDbConnectionCore(connectionString));
                }
            }

            return conn;
        }

        public Connector GetDbConnectionByName(string connectionName, bool usePool = true)
        {
            var connectionString = GetConnectionString(connectionName);
            Connector conn;
            if (!usePool)
            {
                conn = new Connector(GetDbConnectionCore(connectionString));
            }
            else
            {
                conn = GetDbConnectionByPool(connectionString);
                if (conn == null)
                {
                    conn = new Connector(GetDbConnectionCore(connectionString));
                }
            }

            return conn;
        }

        private Connector GetDbConnectionByPool(string connectionString)
        {
            if (!_connectionPools.TryGetValue(connectionString, out var pool))
            {
                if (!_options.TryGetValue(connectionString, out var option))
                {
                    return null;
                }

                try
                {
                    _locker.Enter();
                    pool = new ConnectionPool(new ConnectionPoolPolicy(this, connectionString), option.MaxPool);
                    _connectionPools.Add(connectionString, pool);
                }
                finally
                {
                    _locker.Exit();
                }
            }

            return pool.Get();
        }

        //public IDbConnection GetDbConnection(string connectionString)
        //{
        //    return GetDbConnectionCore(connectionString);
        //}

        //public IDbConnection GetDbConnectionByName(string connectionName)
        //{
        //    var connectionString = GetConnectionString(connectionName);
        //    return GetDbConnectionCore(connectionString);
        //}

        //public IDbConnection GetConnection()
        //{
        //    string connectionString = GetWriteDbConnectionString();
        //    return GetDbConnectionCore(connectionString);
        //}

        //public IDbConnection GetReadOnlyConnection()
        //{
        //    string connectionString = GetRealOnlyDbConnectionString();
        //    return GetDbConnectionCore(connectionString);
        //}


        protected abstract IDbConnection GetDbConnectionCore(string connectionString);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract string GetWriteDbConnectionString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract string GetRealOnlyDbConnectionString();
    }
}
