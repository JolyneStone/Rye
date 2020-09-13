using Monica.DataAccess;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Monica.SqlServer
{
    public abstract class SqlServerConnectionProvider : ConnectionProvider
    {
        protected override IDbConnection GetDbConnectionCore(string connectionString)
        {
            DbConnection connection = SqlClientFactory.Instance.CreateConnection();
            connection.ConnectionString = connectionString;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }
    }
}
