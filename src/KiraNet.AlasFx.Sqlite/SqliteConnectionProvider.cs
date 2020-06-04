using KiraNet.AlasFx.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Data.Common;

namespace KiraNet.AlasFx.Sqlite
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
