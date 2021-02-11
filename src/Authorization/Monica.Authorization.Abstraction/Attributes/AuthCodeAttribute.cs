using System;

namespace Monica.Authorization.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthCodeAttribute : Attribute
    {
        public AuthCodeAttribute(string authCode)
        {
            AuthCode = authCode ?? throw new ArgumentNullException(nameof(authCode));
        }

        public string AuthCode { get; }
    }
}
