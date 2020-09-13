using Monica.DataAccess;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.Common;

namespace Monica.Sqlite
{
    public abstract class SqliteConnectionProvider : ConnectionProvider
    {
        protected override IDbConnection GetDbConnectionCore(string connectionString)
        {
            DbConnection connection = SqliteFactory.Instance.CreateConnection();
            connection.ConnectionString = connectionString;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }
    }
}
