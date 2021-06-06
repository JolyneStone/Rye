using Rye.Enums;
using Rye.Jwt.Entities;
using Rye.Jwt.Options;

using System.Security.Claims;
using System.Threading.Tasks;

namespace Rye.Jwt
{
    /// <summary>
    /// 提供Jwt Token的管理服务
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        /// 获取当前的Jwt配置选项
        /// </summary>
        /// <returns></returns>
        JwtOptions GetCurrentOptions();

        /// <summary>
        /// 获取指定app的配置项
        /// </summary>
        /// <param name="appKey"></param>
        /// <returns></returns>
        JwtOptions GetOptions(string appKey);

        /// <summary>
        /// 创建指定用户的JwtToken信息
        /// </summary>
        /// <param name="tokenEntity"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<JsonWebToken> CreateTokenAsync<T>(T tokenEntity, JwtOptions options = null)
            where T : TokenEntityBase;

        /// <summary>
        /// 使用RefreshToken获取新的JwtToken信息
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<JsonWebToken> RefreshTokenAsync(string refreshToken, JwtOptions options = null);

        /// <summary>
        /// 校验token是否正确
        /// </summary>
        /// <param name="jwtTokenType"></param>
        /// <param name="token"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<ClaimsPrincipal> ValidateTokenAsync(JwtTokenType jwtTokenType, string token, JwtOptions options = null);

        /// <summary>
        /// 从缓存中删除token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="clientType"></param>
        /// <returns></returns>
        Task DeleteTokenAsync(string userId, string clientType);
    }
}
