using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Monica.Jwt.Options
{
    public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
    {
        private readonly IConfiguration _configuration;

        public JwtOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JwtOptions options)
        {
            var section = _configuration.GetSection("Framework:Jwt");
            options = section.Get<JwtOptions>();
            if (options != null)
            {
                //if (jwt.Secret == null)
                //{
                //    jwt.Secret = section.GetValue<string>("Secret");
                //}

                options.Issuer = options.Issuer.IsNullOrEmpty() ? "Monica Identity" : options.Issuer;
                options.Audience = options.Issuer.IsNullOrEmpty() ? "Monica Client" : options.Audience;

                if (options.Encrypt.IsNullOrEmpty())
                {
                    options.Encrypt = SecurityAlgorithms.HmacSha256;
                }
                if (options.Scheme.IsNullOrEmpty())
                {
                    options.Scheme = JwtBearerDefaults.AuthenticationScheme;
                }
                var accessExpireMins = section.GetValue<string>("AccessExpireMins");
                if(accessExpireMins.IsNullOrEmpty() && options.AccessExpireMins <= 0)
                {
                    options.AccessExpireMins = 5;
                }
                var refreshExpireMins = section.GetValue<string>("RefreshExpireMins");
                if (refreshExpireMins.IsNullOrEmpty() && options.RefreshExpireMins <= 0)
                {
                    options.RefreshExpireMins = 10080;
                }
            }
        }
    }
}
