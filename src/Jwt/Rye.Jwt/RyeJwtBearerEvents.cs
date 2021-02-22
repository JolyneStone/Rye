using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Rye.Jwt
{
    public class RyeJwtBearerEvents: JwtBearerEvents
    {
        /// <summary>
        /// 在接收消息时触发，这里定义接收SignalR的token的逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task MessageReceived(MessageReceivedContext context)
        {
            string token = context.Request.Query["access_token"];
            string path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(token) && path.Contains("hub"))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        }

        public override Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true"); // 前端据此判断是否Token过期
            }

            return Task.CompletedTask;
        }
    }
}
