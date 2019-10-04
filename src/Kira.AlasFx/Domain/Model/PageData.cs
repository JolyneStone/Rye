using System;
using System.Collections.Generic;

namespace Kira.AlasFx.Domain.Model
{
    /// <summary>
    /// 分页数据类
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    [Serializable]
    public class PageData<TOutput> where TOutput : IOutput
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }

        public List<TOutput> Data { get; set; }
    }
}
