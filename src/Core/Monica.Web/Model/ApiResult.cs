//namespace Monica.Web
//{
//    public class ApiResult<T>: ApiResult
//    {
//        public ApiResult()
//        {
//        }

//        public ApiResult(T data, string msg, int resultCode, ReturnCodeType returnCode = ReturnCodeType.Success)
//            :base(msg, resultCode, returnCode)
//        {
//            Data = data;
//        }

//        public T Data { get; set; }
//    }

//    /// <summary>
//    /// 表示Api返回结果 
//    /// </summary>
//    public class ApiResult
//    {
//        public ApiResult()
//        { }

//        public ApiResult(string msg, int resultCode, ReturnCodeType returnCode = ReturnCodeType.Success)
//        {
//            Msg = msg;
//            ResultCode = resultCode;
//            ReturnCode = returnCode;
//        }

//        /// <summary>
//        /// 结果标识
//        /// </summary>
//        public int ResultCode { get; set; }

//        /// <summary>
//        /// 通信标识
//        /// </summary>
//        public ReturnCodeType ReturnCode { get; set; }

//        /// <summary>
//        /// 消息内容
//        /// </summary>
//        public string Msg { get; set; }
//    }
//}
