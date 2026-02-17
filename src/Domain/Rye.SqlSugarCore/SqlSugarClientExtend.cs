using SqlSugar;

namespace Rye.SqlSugarCore
{
    /// <summary>
    /// SqlSugar扩展类
    /// </summary>
    public class SqlSugarClientExtend : SqlSugarClient, ISqlSugarClient, ITenant
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config"></param>
        public SqlSugarClientExtend(ConnectionConfig config) : base(config)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configs"></param>
        public SqlSugarClientExtend(List<ConnectionConfig> configs) : base(configs)
        {
        }

        #endregion 构造函数
    }
}
