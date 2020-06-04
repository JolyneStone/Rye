using KiraNet.AlasFx.Domain;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace KiraNet.AlasFx.SqlServer
{
    public abstract class SqlServerConnectionProvider : ConnectionProvider
    {
        public override IDbConnection GetDbConnection(string connectionString, bool open = true)
        {
            DbConnection connection = SqlClientFactory.Instance.CreateConnection();
            connection.ConnectionString = connectionString;
            if (open && connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //protected abstract string GetWriteDbConnectionString();

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //protected abstract string GetRealOnlyDbConnectionString();
    }
}
