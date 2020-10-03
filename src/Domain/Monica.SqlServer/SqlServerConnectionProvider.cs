using Monica.DataAccess;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Monica.DataAccess.Options;

namespace Monica.SqlServer
{
    public abstract class SqlServerConnectionProvider : ConnectionProvider
    {
        protected SqlServerConnectionProvider(IOptions<DbConnectionMapOptions> options) : base(options)
        {
        }

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
