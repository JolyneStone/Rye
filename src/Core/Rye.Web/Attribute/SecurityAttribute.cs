using System;

namespace Rye.Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SecurityAttribute : Attribute
    {
        public SecurityAttribute(bool decryptRequestBody = true, bool encryptResponseBody = true)
        {
            DecryptRequestBody = decryptRequestBody;
            EncryptResponseBody = encryptResponseBody;
        }

        /// <summary>
        /// 是否解密请求body
        /// </summary>
        public bool DecryptRequestBody { get; set; }
        /// <summary>
        /// 是否加密响应body
        /// </summary>
        public bool EncryptResponseBody { get; set; }
    }
}
