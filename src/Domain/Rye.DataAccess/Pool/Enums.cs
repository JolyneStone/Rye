using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rye.DataAccess.Pool
{
    /// <summary>
    /// 创建Connection工作模式
    /// </summary>
    public enum CreateMode
    {
        /// <summary>
        /// 静态模式，会创建连接池配置的最小连接数目
        /// </summary>
        StaticCreateMode = 0,
        /// <summary>
        /// 动态模式，每隔一定时间就对连接池进行检测，如果发现连接数量小于最小连接数，则补充相应数量的新连接
        /// </summary>
        DynamicCreateMode = 1
    }
}
