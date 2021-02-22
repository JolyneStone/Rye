using System;

namespace Rye.AspectFlare
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
    public class NonInterceptAttribute : Attribute
    {
    }
}
