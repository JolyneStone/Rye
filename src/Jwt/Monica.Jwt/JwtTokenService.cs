using Monica.Enums;
using Monica.Exceptions;
using Monica.Jwt.Entities;
using Monica.Jwt.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Monica.Jwt
{
    public class JwtTokenService<TEntity, TUserKey, TRoleKey> : IJwtTokenService<TEntity, TUserKey, TRoleKey>
        where TUserKey : IEquatable<TUserKey>
        where TRoleKey : IEquatable<TRoleKey>
        where TEntity: UserPermission<TUserKey,TRoleKey>, new()
    {

        private readonly JwtOptions _jwtOptions;
        private readonly IServiceProvider _provider;
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

        /// <summary>
        /// 初始化一个<see cref="JwtBearerService{TUser, TUserKey}"/>类型的新实例
        /// </summary>
        public JwtTokenService(IServiceProvider provider, IOptions<JwtOptions> options)
        {
            _provider = provider;
            _jwtOptions = options?.Value;
            Check.NotNull(_jwtOptions, nameof(IOptions<JwtOptions>));
        }

        public virtual Task<JsonWebToken> CreateToken(TEntity entity, string clientId, ClientType clientType = ClientType.Browser)
        {
            return Task.FromResult(GenerateToken(entity, clientId.IsNullOrEmpty() ? Guid.NewGuid().ToString() : clientId, clientType));
        }

        public virtual Task<JsonWebToken> RefreshToken(string refreshToken)
        {
            Check.NotNull(refreshToken, nameof(refreshToken));
            TokenValidationParameters parameters = new TokenValidationParameters()
            {
                ValidIssuer = _jwtOptions.Issuer ?? "Monica Identity",
                ValidAudience = _jwtOptions.Audience ?? "Monica Client",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret))
            };
            JwtSecurityToken jwtSecurityToken = _tokenHandler.ReadJwtToken(refreshToken);
            string clientId = jwtSecurityToken.Claims.FirstOrDefault(m => m.Type == "clientId")?.Value;
            if (clientId == null)
            {
                throw new MonicaException("RefreshToken中不包含ClientId声明");
            }

            ClaimsPrincipal principal = _tokenHandler.ValidateToken(refreshToken, parameters, out _);
            ClientType? clientType = jwtSecurityToken.Claims.FirstOrDefault(m => m.Type == "clientType")?.Value.ParseByEnum<ClientType>()
                 ?? ClientType.Browser;

            var entity = new TEntity();
            entity.Set(principal.Claims);
            JsonWebToken token = GenerateToken(entity, clientId, clientType.Value);
            return Task.FromResult(token);
        }

        private JsonWebToken GenerateToken(TEntity entity, string clientId, ClientType clientType)
        {
            Check.NotNull(entity, nameof(entity));

            clientId = clientId.IsNullOrEmpty() ? Guid.NewGuid().ToString() : clientId;
            var claims = entity.GetClaims(DateTime.Now.AddMinutes(_jwtOptions.AccessExpireMins));
            claims.Add(new Claim("clientId", clientId));
            claims.Add(new Claim("clientType", clientType.ToString()));

            // RefreshToken
            var (token, expires) = CreateTokenCore(claims, _jwtOptions, JwtTokenType.RefreshToken);
            // AccessToken
            (token, _) = CreateTokenCore(claims, _jwtOptions, JwtTokenType.AccessToken);

            string refreshTokenStr = token;

            return new JsonWebToken()
            {
                AccessToken = token,
                RefreshToken = refreshTokenStr,
                RefreshUctExpires = expires.ToJsGetTime().ParseByLong()
            };
        }

        protected (string, DateTime) CreateTokenCore(IEnumerable<Claim> claims, JwtOptions options, JwtTokenType tokenType)
        {
            string secret = options.Secret;
            if (secret == null)
            {
                throw new MonicaException("创建JwtToken时Secret为空，Framework:Jwt:Secret节点中进行配置");
            }

            DateTime expires;
            DateTime now = DateTime.UtcNow;
            if (tokenType == JwtTokenType.AccessToken)
            {
                double minutes = options.AccessExpireMins > 0 ? options.AccessExpireMins : 5; //默认5分钟
                expires = now.AddMinutes(minutes);
            }
            else
            {
                //if (refreshToken == null)
                //{
                //    double minutes = options.RefreshExpireMins > 0 ? options.RefreshExpireMins : 10080; // 默认7天
                //    expires = now.AddMinutes(minutes);
                //}
                //else
                //{
                //    expires = refreshToken.EndUtcTime;
                //}
                double minutes = options.RefreshExpireMins > 0 ? options.RefreshExpireMins : 10080; // 默认7天
                expires = now.AddMinutes(minutes);
            }
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            SigningCredentials credentials = new SigningCredentials(key, _jwtOptions.Encrypt);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Audience = options.Audience,
                Issuer = options.Issuer,
                SigningCredentials = credentials,
                NotBefore = now,
                IssuedAt = now,
                Expires = expires
            };
            SecurityToken token = _tokenHandler.CreateToken(descriptor);
            string accessToken = _tokenHandler.WriteToken(token);
            return (accessToken, expires);
        }
    }
}
