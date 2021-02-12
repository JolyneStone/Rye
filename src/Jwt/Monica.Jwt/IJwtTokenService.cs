using Monica.Enums;
using Monica.Jwt.Entities;

using System.Security.Claims;
using System.Threading.Tasks;

namespace Monica.Jwt
{
    /// <summary>
    /// 提供Jwt Token的管理服务
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        /// 创建指定用户的JwtToken信息
        /// </summary>
        /// <param name="requirement"></param>
        /// <returns></returns>
        Task<JsonWebToken> CreateTokenAsync<T>(T tokenEntity)
            where T : TokenEntityBase;

        /// <summary>
        /// 使用RefreshToken获取新的JwtToken信息
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<JsonWebToken> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// 校验token是否正确
        /// </summary>
        /// <param name="jwtTokenType"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ClaimsPrincipal> ValidateTokenAsync(JwtTokenType jwtTokenType, string token);
    }
}
