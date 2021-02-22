using Microsoft.Extensions.Options;

using Rye.Configuration;
using Rye.DataAccess.Options;

using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;

namespace Rye.DataAccess
{
    public abstract class ConnectionProvider : IConnectionProvider
    {
        private readonly object _sync = new object();
        private readonly Dictionary<string, IDbConnection> _connnectionPools = new Dictionary<string, IDbConnection>();
        private readonly DbConnectionMapOptions _options;

        public ConnectionProvider(IOptions<DbConnectionMapOptions> options)
        {
            _options = options.Value;
        }

        public IDbConnection GetConnection()
        {
            string connectString = GetWriteDbConnectionString();
            return GetDbConnectionCore(connectString);
        }

        public IDbConnection GetReadOnlyConnection()
        {
            string connectString = GetRealOnlyDbConnectionString();
            if (string.IsNullOrEmpty(connectString))
            {
                connectString = GetWriteDbConnectionString();
            }

            return GetDbConnectionCore(connectString);
        }

        public virtual string GetConnectionString(string connectionName)
        {
            //return ConfigurationManager.GetSectionValue($"Framework:DbConnections:{connectionName}:ConnectionString");
            return _options[connectionName]?.ConnectionString;
        }

        public IDbConnection GetDbConnection(string connectionString)
        {
            if (!_connnectionPools.TryGetValue(connectionString, out var conn) || conn.State == ConnectionState.Closed)
            {
                lock (_sync)
                {
                    if (!_connnectionPools.TryGetValue(connectionString, out conn))
                    {
                        conn = GetDbConnectionCore(connectionString);
                        _connnectionPools[connectionString] = conn;
                    }
                }
            }
            if (conn.State != ConnectionState.Open)
                conn.Open();
            return conn;
        }

        public IDbConnection GetDbConnectionByName(string connectionName)
        {
            var connectionString = GetConnectionString(connectionName);
            if (!_connnectionPools.TryGetValue(connectionString, out var conn) || conn.State == ConnectionState.Closed)
            {
                lock (_sync)
                {
                    if (!_connnectionPools.TryGetValue(connectionString, out conn))
                    {
                        conn = GetDbConnectionCore(connectionString);
                        _connnectionPools[connectionString] = conn;
                    }
                }
            }
            if (conn.State != ConnectionState.Open)
                conn.Open();
            return conn;
        }

        protected abstract IDbConnection GetDbConnectionCore(string connectionString);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract string GetWriteDbConnectionString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract string GetRealOnlyDbConnectionString();

        public void Dispose()
        {
            if (_connnectionPools != null)
            {
                foreach (var conn in _connnectionPools)
                {
                    if (conn.Value != null && conn.Value.State != ConnectionState.Closed)
                    {
                        conn.Value.Dispose();
                    }
                }
            }
        }
    }
}
