using System;

namespace Rye.AspectFlare
{
    [AttributeUsage(
         AttributeTargets.Class |
         AttributeTargets.Interface |
         AttributeTargets.Method,
         AllowMultiple = true,
         Inherited = true)]
    public abstract class CalledInterceptAttribute : InterceptAttribute, ICalledInterceptor
    {
        public virtual void Called(CalledInterceptContext calledInterceptorContext)
        {
        }
    }
}
