using Rye.Entities;
using SqlSugar;

namespace Rye.SqlSugarCore.Domains
{
    /// <summary>
    /// 系统表信息表
    /// </summary>
    [SugarTable("SYS_TABLEINFO", IsDisabledDelete = true)]
    [SugarIndex("UNIQUE_SYS_TABLEINFO_TENANTID_TABLENAME", nameof(TenantId), OrderByType.Asc, nameof(TableName), OrderByType.Asc, true)]
    public class SysTableInfoTable : SugarEntity, IHaveTenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [SugarColumn(ColumnName = "TENANTID", IsNullable = false)]
        public int TenantId { get; set; } = 1;

        /// <summary>
        /// 表名
        /// </summary>
        [SugarColumn(ColumnName = "TABLENAME", IsNullable = false, Length = 255)]
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// 表描述
        /// </summary>
        [SugarColumn(ColumnName = "DESCRIPTION", IsNullable = true, Length = 1000)]
        public string? Description { get; set; }

        /// <summary>
        /// 字段数量
        /// </summary>
        [SugarColumn(ColumnName = "COLUMNCOUNT", IsNullable = false)]
        public int ColumnCount { get; set; }
    }
}
