namespace Demo.Core.Common
{
    public class Result
    {
        /// <summary>
        /// 业务状态
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 业务信息
        /// </summary>
        public string Message { get; set; }
    }

    public class Result<T> : Result
    {
        /// <summary>
        /// 业务数据
        /// </summary>
        public T Data { get; set; }
    }
}
