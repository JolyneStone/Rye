using Rye.DataAccess;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Rye.DataAccess.Options;

namespace Rye.Sqlite
{
    public abstract class SqliteConnectionProvider : ConnectionProvider
    {
        protected SqliteConnectionProvider(IOptions<DbConnectionMapOptions> options) : base(options)
        {
        }

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
