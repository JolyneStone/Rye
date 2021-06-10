using Rye.DependencyInjection;
using System;

namespace Rye.Web.SignalR
{
    /// <summary>
    /// SignalR集线器配置特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MapHubAttribute : ScanAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pattern"></param>
        public MapHubAttribute(string pattern)
        {
            Pattern = pattern;
        }

        /// <summary>
        /// 配置终点路由地址
        /// </summary>
        public string Pattern { get; set; }
    }
}
