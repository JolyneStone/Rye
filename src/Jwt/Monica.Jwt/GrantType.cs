using System;
using System.Collections.Generic;
using System.Text;

namespace Monica.Jwt
{
    /// <summary>
    /// 授权类型
    /// </summary>
    public class GrantType
    {
        /// <summary>
        /// 用户密码类型
        /// </summary>
        public const string Password = "password";

        /// <summary>
        /// 刷新Token类型
        /// </summary>
        public const string RefreshToken = "refresh_token";
    }
}
