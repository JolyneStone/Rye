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
        AllowMultiple = true,
        Inherited = true)]
    public abstract class CallingInterceptAttribute : Attribute, ICallingInterceptor
    {
        public int Order { get; set; }
        public virtual void Calling(CallingInterceptContext callingInterceptorContext)
        {
        }
    }
}
