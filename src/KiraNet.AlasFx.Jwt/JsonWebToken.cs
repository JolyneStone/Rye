using System;
using System.Collections.Generic;
using System.Text;

namespace KiraNet.AlasFx.Jwt
{
    /// <summary>
    /// JwtToken模型
    /// </summary>
    public class JsonWebToken
    {
        /// <summary>
        /// 用于业务身份认证的AccessToken
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 用于刷新AccessToken的RefreshToken
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// RefreshToken有效期，UTC标准
        /// </summary>
        public long RefreshUctExpires { get; set; }

        /// <summary>
        /// 刷新Token是否过期
        /// </summary>
        public bool IsRefreshExpired()
        {
            DateTime now = DateTime.Now;
            long nowTick = now.ToJsGetTime().TryParseByLong();
            return RefreshUctExpires > nowTick;
        }
    }
}
