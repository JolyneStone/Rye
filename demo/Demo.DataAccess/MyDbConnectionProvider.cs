using Demo.Core.Common.Enums;
using Microsoft.Extensions.Options;
using Monica;
using Monica.DataAccess.Options;
using Monica.MySql;

namespace Demo.DataAccess
{
    public class MyDbConnectionProvider : MySqlConnectionProvider
    {
        public MyDbConnectionProvider(IOptions<DbConnectionMapOptions> options) : base(options)
        {
        }

        protected override string GetRealOnlyDbConnectionString()
        {
            return GetConnectionString(DbConfig.DbMonica_Read.GetDescription());
        }

        protected override string GetWriteDbConnectionString()
        {
            return GetConnectionString(DbConfig.DbMonica.GetDescription());
        }
    }
}
