using Rye.Entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rye.SqlSugarCore.Domains
{
    /// <summary>
    /// 系统表字段信息表
    /// </summary>
    [SugarTable("SYS_TABLECOLUMN", IsDisabledDelete = true)]
    [SugarIndex("UNIQUE_SYS_TABLECOLUMN_TENANTID_TABLEID_COLUMNNAME", nameof(TenantId), OrderByType.Asc, nameof(TableId), OrderByType.Asc, nameof(ColumnName), OrderByType.Asc, true)]
    public class SysTableColumnTable: SugarEntity, IHaveTenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [SugarColumn(ColumnName = "TENANTID", IsNullable = true)]
        public int TenantId { get; set; } = 1;

        /// <summary>
        /// 表Id
        /// </summary>
        [SugarColumn(ColumnName = "TABLEID", IsNullable = false)]
        public long TableId { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        [SugarColumn(ColumnName = "COLUMNNAME", IsNullable = false, Length = 255)]
        public string ColumnName { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        [SugarColumn(ColumnName = "COLUMNTYPE", IsNullable = false, Length = 64)]
        public string ColumnType { get; set; }

        /// <summary>
        /// 字段长度
        /// </summary>
        [SugarColumn(ColumnName = "LENGTH", IsNullable = false)]
        public int Length { get; set; }

        /// <summary>
        /// 小数位数
        /// </summary>
        [SugarColumn(ColumnName = "DECIMALDIGITS", IsNullable = false)]
        public int DecimalDigits { get; set; }

        /// <summary>
        /// 是否主键
        /// </summary>
        [SugarColumn(ColumnName = "ISPRIMARYKEY", IsNullable = false)]
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// 是否可为空
        /// </summary>
        [SugarColumn(ColumnName = "ISNULLABLE", IsNullable = false)]
        public bool IsNullable { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        [SugarColumn(ColumnName = "DESCRIPTION", IsNullable = false, Length = 1000)]
        public string Description { get; set; }
    }
}
