//using Monica.Web.Enums;

//namespace Monica.Web.Options
//{
//    /// <summary>
//    /// 定义响应结构
//    /// </summary>
//    public class MonicaResponse
//    {
//        /// <summary>
//        /// 业务状态，默认为<see cref="ResultCode"/>
//        /// </summary>
//        public string ResultCode { get; set; } = nameof(ResultCode);
//        /// <summary>
//        /// 成功时返回业务状态值，默认为<see cref="DefaultCodeType.Success"/>
//        /// </summary>
//        public int SuccessResultCode { get; set; } = (int)DefaultCodeType.Success;
//        /// <summary>
//        /// 失败时返回业务状态值，默认为<see cref="DefaultCodeType.Fail"/>
//        /// </summary>
//        public int FailResultCode { get; set; } = (int)DefaultCodeType.Fail;
//        /// <summary>
//        /// 网络状态，默认为<see cref="HttpCode"/>
//        /// </summary>
//        public string HttpCode { get; set; } = nameof(HttpCode);
//        /// <summary>
//        /// 成功时返回网络状态值，默认为<see cref="DefaultHttpCodeType.Success"/>
//        /// </summary>
//        public int SuccessHttpCode { get; set; } = (int)DefaultHttpCodeType.Success;
//        /// <summary>
//        /// 失败时返回网络状态值，默认为<see cref="DefaultHttpCodeType.ServerUnavailable"/>
//        /// </summary>
//        public int FailHttpCode { get; set; } = (int)DefaultHttpCodeType.ServerUnavailable;
//        /// <summary>
//        /// 返回信息，默认为<see cref="Message"/>
//        /// </summary>
//        public string Message { get; set; } = nameof(Message);
//        /// <summary>
//        /// 网络信息，默认为<see cref="HttpMessage"/>
//        /// </summary>
//        public string HttpMessage { get; set; } = nameof(HttpMessage);
//        /// <summary>
//        /// 返回数据，默认为<see cref="Data"/>
//        /// </summary>
//        public string Data { get; set; } = nameof(Data);
//    }
//}