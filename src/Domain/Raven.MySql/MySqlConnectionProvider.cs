using Raven.DataAccess;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace Raven.SqlServer
{
    public abstract class MySqlConnectionProvider : ConnectionProvider
    {
        public override IDbConnection GetDbConnection(string connectionString, bool open = true)
        {
            DbConnection connection = MySqlClientFactory.Instance.CreateConnection();
            connection.ConnectionString = connectionString;
            if (open && connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }
    }
}
