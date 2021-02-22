using System;

namespace Rye.AspectFlare
{
    [AttributeUsage(
        AttributeTargets.Class |
        AttributeTargets.Interface |
        AttributeTargets.Method,
        AllowMultiple = true,
        Inherited = true)]
    public abstract class CallingInterceptAttribute : InterceptAttribute, ICallingInterceptor
    {
        public virtual void Calling(CallingInterceptContext callingInterceptorContext)
        {
        }
    }
}
