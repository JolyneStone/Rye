using MySql.Data.MySqlClient;
using Monica.DataAccess;
using System.Data;
using Microsoft.Extensions.Options;
using Monica.DataAccess.Options;

namespace Monica.MySql
{
    public abstract class MySqlConnectionProvider : ConnectionProvider
    {
        protected MySqlConnectionProvider(IOptions<DbConnectionMapOptions> options) : base(options)
        {
        }

        protected override IDbConnection GetDbConnectionCore(string connectionString)
        {
            var connection = MySqlClientFactory.Instance.CreateConnection();
            connection.ConnectionString = connectionString;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            return connection;
        }
    }
}
