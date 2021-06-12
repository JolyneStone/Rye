using Demo.Common.Enums;

using Microsoft.Extensions.Options;

using Rye;
using Rye.DataAccess.Options;
using Rye.MySql;

namespace Demo.DataAccess
{
    public class MyDbConnectionProvider : MySqlConnectionProvider
    {
        public MyDbConnectionProvider(IOptions<DbConnectionMapOptions> options) : base(options)
        {
        }

        protected override string GetRealOnlyDbConnectionString()
        {
            return GetConnectionString(DbConfig.DbRye_Read.GetDescription());
        }

        protected override string GetWriteDbConnectionString()
        {
            return GetConnectionString(DbConfig.DbRye.GetDescription());
        }
    }
}
