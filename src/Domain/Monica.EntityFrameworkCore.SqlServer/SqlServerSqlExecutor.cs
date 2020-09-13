using Microsoft.EntityFrameworkCore;
using Monica.DataAccess;
using Monica.Enums;
using System.Data;

namespace Monica.EntityFrameworkCore.SqlServer
{
    public class SqlServerSqlExecutor : SqlExecutorBase
    {
        public SqlServerSqlExecutor(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public SqlServerSqlExecutor(IDbContext dbContext) : base(dbContext)
        {
        }

        public SqlServerSqlExecutor(DbContext dbContext) : base(dbContext)
        {
        }

        public SqlServerSqlExecutor(string connectionString) : base(connectionString)
        {
        }

        public override DatabaseType DatabaseType => DatabaseType.SqlServer;

        protected override IDbConnection GetDbConnection(string connectionString)
        {
            var conn= new Microsoft.Data.SqlClient.SqlConnection(connectionString);
            if (conn.State != ConnectionState.Open)
                conn.Open();
            return conn;
        }
    }
}
