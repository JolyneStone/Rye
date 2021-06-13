using Microsoft.Extensions.Logging;

using Rye.AspectFlare;

using System;

namespace Rye
{
    /// <summary>
    /// 日志记录特性，使用AOP拦截
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class LogAttribute : InterceptAttribute, ICallingInterceptor, ICalledInterceptor, IExceptionInterceptor
    {
        private ILogger _logger;

        public void Calling(CallingInterceptContext callingInterceptorContext)
        {
            if (_logger == null)
            {
                _logger = App.GetService<ILoggerFactory>().CreateLogger(callingInterceptorContext.Owner.GetType().Name);
            }

            _logger.LogInformation($"{callingInterceptorContext.MethodName} invoking, arguments: {(callingInterceptorContext.Parameters == null || callingInterceptorContext.Parameters.Length <= 0 ? string.Empty : callingInterceptorContext.Parameters.ToJsonString())}");
        }

        public void Called(CalledInterceptContext calledInterceptorContext)
        {
            if (calledInterceptorContext.ReturnValue != null)
            {
                _logger.LogInformation($"{calledInterceptorContext.MethodName} invoked, result: {calledInterceptorContext.ReturnValue.ToJsonString()}");
            }
        }

        public void Exception(ExceptionInterceptContext exceptionInterceptorContext)
        {
            _logger.LogInformation($"{exceptionInterceptorContext.MethodName} invoke error, exception: {exceptionInterceptorContext.Exception}");
        }
    }
}
