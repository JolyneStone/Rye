using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Kira.AlasFx.Domain;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Kira.AlasFx.EntityFrameworkCore.MySql
{
    public class MySqlExecutor : SqlExecutorBase
    {
        public MySqlExecutor(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public MySqlExecutor(IDbContext dbContext) : base(dbContext)
        {
        }

        public MySqlExecutor(DbContext dbContext) : base(dbContext)
        {
        }

        public MySqlExecutor(string connectionString) : base(connectionString)
        {
        }

        public override DatabaseType DatabaseType => DatabaseType.MySql;

        protected override IDbConnection GetDbConnection(string connectionString)
        {
            var conn = new MySqlConnection(connectionString);
            if (conn.State != ConnectionState.Open)
                conn.Open();
            return conn;
        }
    }
}
