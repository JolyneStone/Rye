using System;
using Monica.AspectFlare;

namespace Simples
{
    public class ExceptionAttribute : ExceptionInterceptAttribute
    {
        public override void Exception(ExceptionInterceptContext exceptionInterceptorContext)
        {
            Console.WriteLine("An exception was thrown: " + exceptionInterceptorContext.Exception.Message);
            exceptionInterceptorContext.HasHandled = true;
        }
    }
}
