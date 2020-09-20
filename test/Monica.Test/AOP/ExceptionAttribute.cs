using System;
using System.Diagnostics;

using Monica.AspectFlare;

namespace Monica.Test.AOP
{
    public class ExceptionAttribute : ExceptionInterceptAttribute
    {
        public override void Exception(ExceptionInterceptContext exceptionInterceptorContext)
        {
            Debug.WriteLine("An exception was thrown: " + exceptionInterceptorContext.Exception.Message);
            exceptionInterceptorContext.HasHandled = true;
        }
    }
}
