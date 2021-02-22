using Rye.DataAccess;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Rye.DataAccess.Options;

namespace Rye.SqlServer
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
