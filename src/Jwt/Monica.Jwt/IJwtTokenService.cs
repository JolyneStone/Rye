using Monica.Enums;

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
        /// <param name="clientType"></param>
        /// <returns></returns>
        JsonWebToken CreateToken<T>(T tokenEntity, ClientType clientType = ClientType.Browser)
            where T : class;

        /// <summary>
        /// 使用RefreshToken获取新的JwtToken信息
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        JsonWebToken RefreshToken(string refreshToken);

        /// <summary>
        /// 校验token是否正确
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ClaimsPrincipal ValidateToken(string token);
    }
}
