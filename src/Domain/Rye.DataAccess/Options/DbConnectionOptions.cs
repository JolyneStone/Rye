using Rye.Enums;

namespace Rye.DataAccess.Options
{
    ///<summary>
    /// 数据库配置选项
    /// </summary>
    public class DbConnectionOptions
    {
        /// <summary>
        /// 数据库字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DatabaseType DatabaseType { get; set; }

        /// <summary>
        /// 数据库连接池最大容量
        /// </summary>
        public int MaxPool { get; set; } = 100;
    }
}
