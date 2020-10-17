using System;

namespace Monica.AspectFlare
{
    [AttributeUsage(
         AttributeTargets.Class |
         AttributeTargets.Interface |
         AttributeTargets.Method,
         AllowMultiple = false,
         Inherited = true)]
    public abstract class ExceptionInterceptAttribute : InterceptAttribute, IExceptionInterceptor
    {
        public virtual void Exception(ExceptionInterceptContext exceptionInterceptorContext)
        {
        }
    }
}
