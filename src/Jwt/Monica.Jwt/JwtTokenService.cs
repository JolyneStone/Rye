using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Monica.Enums;
using Monica.Exceptions;
using Monica.Jwt.Options;

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
using System.Text.Unicode;

namespace Monica.Jwt
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

        public JwtTokenService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        /// <summary>
        /// 校验token是否正确
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual ClaimsPrincipal ValidateToken(string token)
        {
            ClaimsPrincipal principal = _tokenHandler.ValidateToken(token, _jwtOptions.GetValidationParameters(), out _);
            return principal;
        }

        public virtual JsonWebToken CreateToken<T>(T tokenEntity, ClientType clientType = ClientType.Browser)
            where T : class
        {
            return GenerateToken(GetClaims(tokenEntity), clientType);
        }

        public virtual JsonWebToken RefreshToken(string refreshToken)
        {
            Check.NotNull(refreshToken, nameof(refreshToken));
            TokenValidationParameters parameters = _jwtOptions.GetValidationParameters();
            JwtSecurityToken jwtSecurityToken = _tokenHandler.ReadJwtToken(refreshToken);
            string clientId = jwtSecurityToken.Claims.FirstOrDefault(m => m.Type == "clientId")?.Value;
            if (clientId == null)
            {
                throw new MonicaException("RefreshToken中不包含ClientId声明");
            }

            ClaimsPrincipal principal = _tokenHandler.ValidateToken(refreshToken, parameters, out _);
            ClientType? clientType = jwtSecurityToken.Claims.FirstOrDefault(m => m.Type == "ClientType")?.Value.ParseByEnum<ClientType>()
                 ?? ClientType.Browser;

            JsonWebToken token = GenerateToken(principal.Claims.ToList(), clientType.Value);
            return token;
        }

        private JsonWebToken GenerateToken(List<Claim> claims, ClientType clientType)
        {
            if (claims == null)
                claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"));
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddMinutes(_jwtOptions.AccessExpireMins)).ToUnixTimeSeconds()}"));
            claims.Add(new Claim("ClientType", clientType.ToString()));

            // AccessToken
            var (accessToken, accessExpires) = CreateTokenCore(claims, _jwtOptions, JwtTokenType.AccessToken);

            // RefreshToken
            var (refreshToken, refreshExpires) = CreateTokenCore(claims, _jwtOptions, JwtTokenType.RefreshToken);
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

                claims = claims.Append(new Claim("TokenType", ((int)JwtTokenType.RefreshToken).ToString()));
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

        private static List<Claim> GetClaims<T>(T requirement)
            where T : class
        {
            if (requirement == null)
            {
                return new List<Claim>();
            }

            //var serializerOptions = new JsonSerializerOptions
            //{
            //    PropertyNameCaseInsensitive = false,
            //    NumberHandling = JsonNumberHandling.AllowReadingFromString,
            //    WriteIndented = true,
            //    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            //};
            //var dic = requirement.ToJsonString(serializerOptions).ToObject<Dictionary<string, JsonElement>>(serializerOptions);
            //return dic.Select(d => new Claim(
            //    d.Key,
            //    d.Value.ValueKind switch
            //    {
            //        JsonValueKind.Object => d.Value.ToJsonString(),
            //        JsonValueKind.Null => string.Empty,
            //        JsonValueKind.Array => string.Join(",", d.Value.EnumerateArray().Select(e => e.GetString())),
            //        _=> d.Value.ToString()
            //    }
            //)).ToList();
            var claims = new List<Claim>();
            foreach(var property in requirement.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty))
            {
                var value = property.GetValue(requirement);
                if (value == null)
                {
                    claims.Add(new Claim(property.Name, string.Empty));
                }
                else if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
                {
                    claims.Add(new Claim(property.Name, value.ToString()));
                }
                else
                {
                    claims.Add(new Claim(property.Name, value.ToJsonString()));
                }
            }
            return claims;
        }
    }
}
