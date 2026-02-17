using System;
using System.Collections.Generic;
using System.Text;

namespace Rye.SqlSugarCore.Util
{
    /// <summary>
    /// 表字典类
    /// </summary>
    public class TableDict
    {
        #region 字段类型字典

        /// <summary>
        /// 字段类型字典
        /// </summary>
        public static readonly Dictionary<Type, string> ColumnTypeDict = new()
        {
            { typeof(bool), "BIT" },
            { typeof(bool?), "BIT" },
            { typeof(byte), "TINYINT" },
            { typeof(byte?), "TINYINT" },
            { typeof(short), "SMALLINT" },
            { typeof(short?), "SMALLINT" },
            { typeof(int), "INT" },
            { typeof(int?), "INT" },
            { typeof(long), "BIGINT" },
            { typeof(long?), "BIGINT" },
            { typeof(double), "FLOAT" },
            { typeof(double?), "FLOAT" },
            { typeof(float), "REAL" },
            { typeof(float?), "REAL" },
            { typeof(decimal), "DECIMAL" },
            { typeof(decimal?), "DECIMAL" },
            { typeof(string), "VARCHAR" },
            { typeof(DateTime), "DATETIME" },
            { typeof(DateTime?), "DATETIME" },
            { typeof(byte[]), "TIMESTAMP" },
            { typeof(Guid), "UNIQUEIDENTIFIER" },
            { typeof(Guid?), "UNIQUEIDENTIFIER" },
        };

        #endregion 字段类型字典
    }
}
