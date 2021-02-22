using System;
using System.Diagnostics;

using Rye.AspectFlare;

namespace Rye.Test.AOP
{
    public class ExceptionAttribute : ExceptionInterceptAttribute
    {
        public override void Exception(ExceptionInterceptContext exceptionInterceptorContext)
        {
            Debug.WriteLine("An exception was thrown: " + exceptionInterceptorContext.Exception.Message);
        }
    }
}
