namespace KiraNet.AlasFx.Filter
{
    /// <summary>
    /// 分页查询条件
    /// </summary>
    public class PageCondition
    {
        /// <summary>
        /// 默认参数（第1页，每页10，排序条件为空）
        /// </summary>
        public PageCondition(): this(1, 20)
        { 
        }

        public PageCondition(int pageIndex, int pageSize)
        {
            Check.CheckGreaterThan(pageIndex, 0, false, nameof(pageIndex));
            Check.CheckGreaterThan(pageSize, 0, false, nameof(pageSize));
            PageIndex = pageIndex;
            PageSize = pageSize;
            SortConditions = new SortCondition[] { };
        }

        /// <summary>
        /// 获取或设置 页索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 获取或设置 页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 获取或设置 排序条件组
        /// </summary>
        public SortCondition[] SortConditions { get; set; }
    }
}
