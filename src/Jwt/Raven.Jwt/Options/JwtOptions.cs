using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Raven.Jwt.Options
{
    /// <summary>
    /// JWT身份认证选项
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// 发行方
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 订阅方
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// AccessToken有效期分钟数，默认为5分钟
        /// </summary>
        public double AccessExpireMins { get; set; }

        /// <summary>
        /// RefreshToken有效期分钟数，默认为10080分钟，即7天
        /// </summary>
        public double RefreshExpireMins { get; set; }

        /// <summary>
        /// Token是否过期，默认为不过期
        /// </summary>
        public bool IsExpire { get; set; } = false;

        /// <summary>
        /// 加密算法，默认为HMACSHA256
        /// </summary>
        public string Encrypt { get; set; } = SecurityAlgorithms.HmacSha256;

        /// <summary>
        /// 方案名称，默认为Bearer
        /// </summary>
        public string Scheme { get; set; } = JwtBearerDefaults.AuthenticationScheme;
    }
}