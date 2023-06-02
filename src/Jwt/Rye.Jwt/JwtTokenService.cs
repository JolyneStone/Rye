using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Rye.Cache;
using Rye.Cache.Store;
using Rye.Exceptions;
using Rye.Jwt.Entities;
using Rye.Jwt.Options;
using Rye.Web;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Rye.Jwt
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

        private readonly ICacheStore _store;
        public JwtTokenService(IOptions<JwtOptions> jwtOptions, ICacheStore store)
        {
            _jwtOptions = jwtOptions.Value;
            _store = store;
        }

        public JwtOptions GetCurrentOptions()
        {
            var appKey = WebApp.HttpContext?.Request.GetString("appKey");
            return GetOptions(appKey);
        }

        public JwtOptions GetOptions(string appKey)
        {
            return new JwtOptions
            {
                Secret = _jwtOptions.Secret,
                Scheme = _jwtOptions.Scheme,
                Encrypt = _jwtOptions.Encrypt,
                IsExpire = _jwtOptions.IsExpire,
                Issuer = _jwtOptions.Issuer,
                AccessExpireMins = _jwtOptions.AccessExpireMins,
                RefreshExpireMins = _jwtOptions.RefreshExpireMins,
                Audience = appKey.IsNullOrEmpty() ? _jwtOptions.Audience : appKey,
                Cache = _jwtOptions.Cache
            };
        }

        /// <summary>
        /// 校验token是否正确
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual async Task<ClaimsPrincipal> ValidateTokenAsync(JwtTokenType jwtTokenType, string token, JwtOptions options = null)
        {
            if (options == null)
            {
                options = GetCurrentOptions();
            }
            ClaimsPrincipal principal = _tokenHandler.ValidateToken(token, options.GetValidationParameters(), out _);
            string userId = null;
            if (options.EnabledSignalR)
            {
                userId = principal.Claims.FirstOrDefault(d => d.Type == nameof(TokenEntityBase.UserId)).Value;
                principal.AddIdentity(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                }));
            }
            if (options.Cache)
            {
                var clientType = principal.Claims.FirstOrDefault(d => d.Type == nameof(TokenEntityBase.ClientType)).Value;
                if (userId == null)
                    userId = principal.Claims.FirstOrDefault(d => d.Type == nameof(TokenEntityBase.UserId)).Value;
                var tokenEntry = CacheEntryCollection.GetTokenEntry(jwtTokenType, clientType, userId, (int)options.AccessExpireMins * 60);

                var cacheToken = await _store.GetAsync<string>(tokenEntry);
                if (cacheToken.IsNullOrEmpty() || cacheToken != token)
                {
                    throw new RyeException("Token is error");
                }
            }
            return principal;
        }

        public virtual async Task<JsonWebToken> CreateTokenAsync<T>(T tokenEntity, JwtOptions options = null)
            where T : TokenEntityBase
        {
            if (options == null)
            {
                options = GetCurrentOptions();
            }
            var claims = GetClaims(tokenEntity);
            return await GenerateTokenAsync(claims, options);
        }

        public virtual async Task<JsonWebToken> RefreshTokenAsync(string refreshToken, JwtOptions options = null)
        {
            Check.NotNull(refreshToken, nameof(refreshToken));
            if (options == null)
            {
                options = GetCurrentOptions();
            }
            TokenValidationParameters parameters = options.GetValidationParameters();
            JwtSecurityToken jwtSecurityToken = _tokenHandler.ReadJwtToken(refreshToken);
            string clientId = jwtSecurityToken.Claims.FirstOrDefault(m => m.Type == nameof(TokenEntityBase.ClientType))?.Value;
            if (clientId == null)
            {
                throw new RyeException($"RefreshToken中不包含{nameof(TokenEntityBase.ClientType)}声明");
            }

            ClaimsPrincipal principal = await ValidateTokenAsync(JwtTokenType.RefreshToken, refreshToken, options);

            return await GenerateTokenAsync(principal.Claims.ToList(), options);
        }

        public virtual async Task DeleteTokenAsync(string userId, string clientType)
        {
            var tokenEntry = CacheEntryCollection.GetTokenEntry(JwtTokenType.AccessToken, clientType, userId);
            await _store.RemoveAsync(tokenEntry.Key);

            tokenEntry = CacheEntryCollection.GetTokenEntry(JwtTokenType.RefreshToken, clientType, userId);
            await _store.RemoveAsync(tokenEntry.Key);
        }

        private async Task<JsonWebToken> GenerateTokenAsync(List<Claim> claims, JwtOptions options)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}"));
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, $"{DateTimeOffset.UtcNow.AddMinutes(options.AccessExpireMins).ToUnixTimeSeconds()}"));

            // AccessToken
            var (accessToken, accessExpires) = CreateTokenCore(claims, options, JwtTokenType.AccessToken);

            // RefreshToken
            var (refreshToken, refreshExpires) = CreateTokenCore(claims, options, JwtTokenType.RefreshToken);

            if (options.Cache)
            {
                var clientType = claims.FirstOrDefault(d => d.Type == nameof(TokenEntityBase.ClientType)).Value;
                var userId = claims.FirstOrDefault(d => d.Type == nameof(TokenEntityBase.UserId)).Value;

                var tokenEntry = CacheEntryCollection.GetTokenEntry(JwtTokenType.AccessToken, clientType, userId, (int)options.AccessExpireMins * 60);
                await _store.SetAsync<string>(tokenEntry, accessToken);

                tokenEntry = CacheEntryCollection.GetTokenEntry(JwtTokenType.RefreshToken, clientType, userId, (int)options.AccessExpireMins * 60);
                await _store.SetAsync<string>(tokenEntry, accessToken);
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
            SigningCredentials credentials = new SigningCredentials(key, options.Encrypt);

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
