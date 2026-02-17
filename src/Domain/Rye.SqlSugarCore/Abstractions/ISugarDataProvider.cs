namespace Rye.SqlSugarCore
{
    /// <summary>
    /// 数据初始化接口类
    /// </summary>
    public interface ISugarDataProvider
    {
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        Task InitAsync();
    }
}
