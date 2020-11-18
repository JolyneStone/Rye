//using Monica.Web.Options;
//using System.Collections.Generic;

//namespace Monica.Web.Internal
//{
//    internal static class ResponseExtensions
//    {
//        /// <summary>
//        /// 无效参数的响应
//        /// </summary>
//        /// <param name="response"></param>
//        /// <param name="message"></param>
//        /// <param name="httpMessage"></param>
//        /// <returns></returns>
//        internal static Dictionary<string, object> ParametersInvalid(this MonicaResponse response, string message, string httpMessage)
//        {
//            var result = new Dictionary<string, object>();
//            if (!string.IsNullOrEmpty(response.ResultCode))
//            {
//                result.Add(response.ResultCode, response.FailResultCode);
//            }
//            if (!string.IsNullOrEmpty(response.Message))
//            {
//                result.Add(response.Message, string.IsNullOrEmpty(message) ? "Parameters invalid" : $"Parameters invalid: {message}");
//            }
//            if (!string.IsNullOrEmpty(response.HttpCode))
//            {
//                result.Add(response.ResultCode, response.FailHttpCode);
//            }
//            if (!string.IsNullOrEmpty(response.HttpMessage))
//            {
//                result.Add(response.Message, string.IsNullOrEmpty(httpMessage) ? "Fail" : httpMessage);
//            }
//            if (!string.IsNullOrEmpty(response.Data))
//            {
//                result.Add(response.Data, new object());
//            }

//            return result;
//        }

//        /// <summary>
//        /// 重复提交的响应
//        /// </summary>
//        /// <param name="response"></param>
//        /// <param name="message"></param>
//        /// <param name="httpMessage"></param>
//        /// <returns></returns>
//        internal static Dictionary<string, object> RepeatableCommit(this MonicaResponse response, string message, string httpMessage)
//        {
//            var result = new Dictionary<string, object>();
//            if (!string.IsNullOrEmpty(response.ResultCode))
//            {
//                result.Add(response.ResultCode, response.FailResultCode);
//            }
//            if (!string.IsNullOrEmpty(response.Message))
//            {
//                result.Add(response.Message, string.IsNullOrEmpty(message) ? "You can't repeat the submission, please try again later!" : message);
//            }
//            if (!string.IsNullOrEmpty(response.HttpCode))
//            {
//                result.Add(response.ResultCode, response.FailHttpCode);
//            }
//            if (!string.IsNullOrEmpty(response.HttpMessage))
//            {
//                result.Add(response.Message, string.IsNullOrEmpty(httpMessage) ? "Fail" : httpMessage);
//            }
//            if (!string.IsNullOrEmpty(response.Data))
//            {
//                result.Add(response.Data, new object());
//            }

//            return result;
//        }
//    }
//}
