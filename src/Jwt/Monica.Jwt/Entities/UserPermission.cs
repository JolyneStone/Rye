using Monica.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Monica.Jwt.Entities
{
    public class UserPermission<TUserKey, TRoleKey> : IAuthorizationRequirement
        where TUserKey : IEquatable<TUserKey>
        where TRoleKey : IEquatable<TRoleKey>
    {
        public UserPermission()
        {
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public TUserKey UserId { get; set; }

        /// <summary>
        /// 角色Id集合
        /// </summary>
        public List<TRoleKey> RoleIds { get; set; }

        /// <summary>
        /// 返回声明列表
        /// </summary>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        public virtual List<Claim> GetClaims(DateTime expireTime)
        {
            return new List<Claim>()
              {
                    new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                    new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(expireTime).ToUnixTimeSeconds()}"),
                    new Claim(JwtRegisteredClaimNames.Sid, UserId.ToString()),
                    new Claim("Role", RoleIds.ExpandAndToString(","))
                };
        }

        public virtual void Set(IEnumerable<Claim> claims)
        {
            Check.NotNull(claims, nameof(claims));
            string sid = claims.FirstOrDefault(m => m.Type == JwtRegisteredClaimNames.Sid)?.Value;
            if (sid.IsNullOrEmpty())
            {
                throw new MonicaException($"声明集合中无法找到{JwtRegisteredClaimNames.Sid}声明");
            }
            string roles = claims.FirstOrDefault(m => m.Type == "Role")?.Value;
            if (roles.IsNullOrEmpty())
            {
                throw new MonicaException("声明集合中无法找到Role声明");
            }

            UserId = sid.Parse<TUserKey>();
            RoleIds = roles.Split(",").Select(d => d.Parse<TRoleKey>()).ToList();
        }
    }
}
