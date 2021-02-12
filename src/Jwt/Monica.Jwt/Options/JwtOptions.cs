using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Monica.Jwt.Options
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
        public double AccessExpireMins { get; set; } = 5;

        /// <summary>
        /// RefreshToken有效期分钟数，默认为10080分钟，即7天
        /// </summary>
        public double RefreshExpireMins { get; set; } = 10080;

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

        /// <summary>
        /// 是否使用缓存
        /// </summary>
        public bool Cache { get; set; } = true; 

        /// <summary>
        /// 获取TokenValidationParameters
        /// </summary>
        /// <returns></returns>
        public TokenValidationParameters GetValidationParameters()
        {
            var parameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)), // key值长度至少是16
                ValidateIssuer = true,
                //IssuerValidator = (string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters) => Issuer,
                ValidIssuer = Issuer,
                RequireSignedTokens = false,
                ValidateAudience = true,
                ValidAudience = Audience,
                ValidateLifetime = IsExpire,
                RequireExpirationTime = IsExpire,
                ClockSkew = TimeSpan.FromMinutes(AccessExpireMins),

                LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
                SignatureValidator = (token, param) => new JwtSecurityToken(token),
            };

            return parameters;
        }
    }
}