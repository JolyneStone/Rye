using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rye.SqlSugarCore
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SugarEntity: ISugarTableProvider
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true, IsIdentity = true, IsNullable = false, CreateTableFieldSort = -10000)]
        public uint Id { get; set; }
    }
}
