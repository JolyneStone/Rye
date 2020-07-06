using Raven.Configuration;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Raven.DataAccess
{
    public abstract class ConnectionProvider : IConnectionProvider
    {
        private readonly object _sync = new object();
        private readonly Dictionary<string, IDbConnection> _connnectionPools = new Dictionary<string, IDbConnection>();
        public IDbConnection GetConnection()
        {
            string connectString = GetConnectionString(GetWriteDbConnectionString());
            return GetDbConnectionCore(connectString);
        }

        public IDbConnection GetReadOnlyConnection()
        {
            string connectString = GetConnectionString(GetRealOnlyDbConnectionString());
            if (string.IsNullOrEmpty(connectString))
            {
                connectString = GetConnectionString(GetWriteDbConnectionString());
            }

            return GetDbConnectionCore(connectString);
        }

        public string GetConnectionString(string connectionName)
        {
            return ConfigurationManager.Appsettings.GetConnectionString(connectionName);
        }

        public IDbConnection GetDbConnection(string connectionString)
        {
            if (!_connnectionPools.TryGetValue(connectionString, out var conn))
            {
                lock (_sync)
                {
                    if (!_connnectionPools.TryGetValue(connectionString, out conn))
                    {
                        conn = GetDbConnectionCore(connectionString);
                        _connnectionPools.Add(connectionString, conn);
                    }
                }
            }
            return conn;
        }

        public IDbConnection GetDbConnectionByName(string connectionName)
        {
            var connectionString = GetConnectionString(connectionName);
            if (!_connnectionPools.TryGetValue(connectionString, out var conn))
            {
                lock (_sync)
                {
                    if (!_connnectionPools.TryGetValue(connectionString, out conn))
                    {
                        conn = GetDbConnectionCore(connectionString);
                        _connnectionPools.Add(connectionString, conn);
                    }
                }
            }
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
                    conn.Value?.Dispose();
                }
            }
        }
    }
}
