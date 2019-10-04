using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.AlasFx.Domain
{
    public enum DatabaseType
    {
        /// <summary>
        /// SqlServer数据库类型
        /// </summary>
        SqlServer = 0,
        /// <summary>
        /// MySql数据库类型
        /// </summary>
        MySql = 1,
        /// <summary>
        /// Sqlite数据库类型
        /// </summary>
        Sqlite = 2
    }
}
