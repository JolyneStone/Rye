using Raven.DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Data.Common;

namespace Raven.Sqlite
{
    public abstract class SqliteConnectionProvider : ConnectionProvider
    {
        public override IDbConnection GetDbConnection(string connectionString, bool open = true)
        {
            DbConnection connection = SqliteFactory.Instance.CreateConnection();
            connection.ConnectionString = connectionString;
            if (open && connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }
    }
}
