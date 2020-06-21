using System;

namespace Raven.AspectFlare
{
    [AttributeUsage(
     AttributeTargets.Struct |
     AttributeTargets.Class |
     AttributeTargets.Constructor |
     AttributeTargets.Event |
     AttributeTargets.Property |
     AttributeTargets.Field |
     AttributeTargets.Interface |
     AttributeTargets.Method,
     AllowMultiple = false,
     Inherited = true)]
    public abstract class ExceptionInterceptAttribute : Attribute, IExceptionInterceptor
    {
        public int Order { get; set; }
        public virtual void Exception(ExceptionInterceptContext exceptionInterceptorContext)
        {
        }
    }
}
