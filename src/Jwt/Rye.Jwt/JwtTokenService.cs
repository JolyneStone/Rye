using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Rye.Cache.Internal;
using Rye.Enums;
using Rye.Exceptions;
using Rye.Jwt.Entities;
using Rye.Jwt.Options;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Rye.Jwt
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
        private readonly IDistributedCache _distributedCache;
        public JwtTokenService(IOptions<JwtOptions> jwtOptions, IDistributedCache distributedCache)
        {
            _jwtOptions = jwtOptions.Value;
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// 校验token是否正确
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual async Task<ClaimsPrincipal> ValidateTokenAsync(JwtTokenType jwtTokenType, string token)
        {
            ClaimsPrincipal principal = _tokenHandler.ValidateToken(token, _jwtOptions.GetValidationParameters(), out _);
            if (_jwtOptions.Cache)
            {
                var clientType = principal.Claims.FirstOrDefault(d => d.Type == nameof(TokenEntityBase.ClientType)).Value;
                var userId = principal.Claims.FirstOrDefault(d => d.Type == nameof(TokenEntityBase.UserId)).Value;
                var tokenEntry = CacheEntryCollection.GetTokenEntry(jwtTokenType, clientType, userId, (int)_jwtOptions.AccessExpireMins * 60);

                var cacheToken = await _distributedCache.GetStringAsync(tokenEntry.CacheKey);
                if (cacheToken.IsNullOrEmpty() || cacheToken != token)
                {
                    throw new RyeException("Token is error");
                }
            }
            return principal;
        }

        public virtual async Task<JsonWebToken> CreateTokenAsync<T>(T tokenEntity)
            where T : TokenEntityBase
        {
            var claims = GetClaims(tokenEntity);
            return await GenerateTokenAsync(claims);
        }

        public virtual async Task<JsonWebToken> RefreshTokenAsync(string refreshToken)
        {
            Check.NotNull(refreshToken, nameof(refreshToken));
            TokenValidationParameters parameters = _jwtOptions.GetValidationParameters();
            JwtSecurityToken jwtSecurityToken = _tokenHandler.ReadJwtToken(refreshToken);
            string clientId = jwtSecurityToken.Claims.FirstOrDefault(m => m.Type == "clientId")?.Value;
            if (clientId == null)
            {
                throw new RyeException("RefreshToken中不包含ClientId声明");
            }

            ClaimsPrincipal principal = _tokenHandler.ValidateToken(refreshToken, parameters, out _);

            return await GenerateTokenAsync(principal.Claims.ToList());
        }

        public virtual async Task DeleteTokenAsync(string userId, string clientType)
        {
            var tokenEntry = CacheEntryCollection.GetTokenEntry(JwtTokenType.AccessToken, clientType, userId);
            await _distributedCache.RemoveAsync(tokenEntry.CacheKey);

            tokenEntry = CacheEntryCollection.GetTokenEntry(JwtTokenType.RefreshToken, clientType, userId);
            await _distributedCache.RemoveAsync(tokenEntry.CacheKey);
        }

        private async Task<JsonWebToken> GenerateTokenAsync(List<Claim> claims)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"));
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddMinutes(_jwtOptions.AccessExpireMins)).ToUnixTimeSeconds()}"));

            // AccessToken
            var (accessToken, accessExpires) = CreateTokenCore(claims, _jwtOptions, JwtTokenType.AccessToken);

            // RefreshToken
            var (refreshToken, refreshExpires) = CreateTokenCore(claims, _jwtOptions, JwtTokenType.RefreshToken);

            if (_jwtOptions.Cache)
            {
                var clientType = claims.FirstOrDefault(d => d.Type == nameof(TokenEntityBase.ClientType)).Value;
                var userId = claims.FirstOrDefault(d => d.Type == nameof(TokenEntityBase.UserId)).Value;

                var tokenEntry = CacheEntryCollection.GetTokenEntry(JwtTokenType.AccessToken, clientType, userId, (int)_jwtOptions.AccessExpireMins * 60);
                await _distributedCache.SetStringAsync(tokenEntry.CacheKey, accessToken, tokenEntry.Options);

                tokenEntry = CacheEntryCollection.GetTokenEntry(JwtTokenType.RefreshToken, clientType, userId, (int)_jwtOptions.AccessExpireMins * 60);
                await _distributedCache.SetStringAsync(tokenEntry.CacheKey, accessToken, tokenEntry.Options);
            }

            return new JsonWebToken()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessExpires = accessExpires.ToUniversalTime().Ticks,
                RefreshExpires = refreshExpires.ToUniversalTime().Ticks//expires.ToJsGetTime().ParseByLong()
            };
        }

        protected (string, DateTime) CreateTokenCore(IEnumerable<Claim> claims, JwtOptions options, JwtTokenType tokenType)
        {
            string secret = options.Secret;
            if (secret == null)
            {
                throw new RyeException("创建JwtToken时Secret为空，Framework:Jwt:Secret节点中进行配置");
            }

            DateTime expires;
            DateTime now = DateTime.UtcNow;
            claims = claims.Append(new Claim("TokenType", ((int)tokenType).ToString()));
            if (tokenType == JwtTokenType.AccessToken)
            {
                double minutes = options.AccessExpireMins > 0 ? options.AccessExpireMins : 5; //默认5分钟
                expires = now.AddMinutes(minutes);
            }
            else
            {
                double minutes = options.RefreshExpireMins > 0 ? options.RefreshExpireMins : 10080; // 默认7天
                expires = now.AddMinutes(minutes);
            }
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            SigningCredentials credentials = new SigningCredentials(key, _jwtOptions.Encrypt);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = expires,
                NotBefore = now,
                IssuedAt = now,
                Audience = options.Audience,
                Issuer = options.Issuer,
            };
            SecurityToken token = _tokenHandler.CreateToken(descriptor);
            string accessToken = _tokenHandler.WriteToken(token);
            return (accessToken, expires);
        }

        private static List<Claim> GetClaims(object requirement)
        {
            if (requirement == null)
            {
                return new List<Claim>();
            }

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            };
            var dic = requirement.ToJsonString(serializerOptions).ToObject<Dictionary<string, JsonElement>>(serializerOptions);
            return dic.Select(d =>
            {
                var tempStr = d.Value.GetRawText();
                return new Claim(
                d.Key,
                tempStr.IsNullOrEmpty() || tempStr.Length < 3 ?
                    "" :
                    new string(tempStr.AsSpan(1, tempStr.Length - 2)));
            }).ToList();
            //var claims = new List<Claim>();
            //foreach (var property in requirement.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty))
            //{
            //    var value = property.GetValue(requirement);
            //    if (value == null)
            //    {
            //        claims.Add(new Claim(property.Name, string.Empty));
            //    }
            //    else if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
            //    {
            //        claims.Add(new Claim(property.Name, value.ToString()));
            //    }
            //    else
            //    {
            //        claims.Add(new Claim(property.Name, value.ToJsonString()));
            //    }
            //}
            //return claims;
        }
    }
}
