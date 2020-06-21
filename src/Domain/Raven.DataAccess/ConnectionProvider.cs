using Raven.Configuration;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Runtime.CompilerServices;

namespace Raven.DataAccess
{
    public abstract class ConnectionProvider : IConnectionProvider
    {
        public IDbConnection GetConnection(bool open = true)
        {
            string connectString = GetConnectionString(GetWriteDbConnectionString());
            return GetDbConnection(connectString, open);
        }

        public IDbConnection GetReadOnlyConnection(bool open = true)
        {
            string connectString = GetConnectionString(GetRealOnlyDbConnectionString());
            if (string.IsNullOrEmpty(connectString))
            {
                connectString = GetConnectionString(GetWriteDbConnectionString());
            }

            return GetDbConnection(connectString, open);
        }

        public string GetConnectionString(string connectionName)
        {
            return ConfigurationManager.Appsettings.GetConnectionString(connectionName);
        }

        public abstract IDbConnection GetDbConnection(string connectionString, bool open = true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract string GetWriteDbConnectionString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract string GetRealOnlyDbConnectionString();
    }
}
