using System;
using System.Collections.Generic;
using System.Text;

namespace KiraNet.AlasFx.Jwt
{
    /// <summary>
    /// 刷新Token信息
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// 客户端Id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 标识值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime EndUtcTime { get; set; }
    }
}
