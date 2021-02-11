using System;

namespace Monica.Authorization.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class LoginAttribute : Attribute
    {
    }
}
