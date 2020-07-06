using Raven.Enums;
using Raven.Jwt.Entities;
using System;
using System.Threading.Tasks;

namespace Raven.Jwt
{
    /// <summary>
    /// 提供Jwt Token的管理服务
    /// </summary>
    public interface IJwtTokenService<TEntity, TUserKey, TRoleKey>
        where TUserKey : IEquatable<TUserKey>
        where TRoleKey : IEquatable<TRoleKey>
        where TEntity: UserPermission<TUserKey, TRoleKey>
    {
        /// <summary>
        /// 创建指定用户的JwtToken信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="clientType"></param>
        /// <returns></returns>
        Task<JsonWebToken> CreateToken(TEntity entity, string clientId, ClientType clientType = ClientType.Browser);

        /// <summary>
        /// 使用RefreshToken获取新的JwtToken信息
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<JsonWebToken> RefreshToken(string refreshToken);
    }
}
