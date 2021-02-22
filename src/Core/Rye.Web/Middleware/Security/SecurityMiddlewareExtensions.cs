using Microsoft.AspNetCore.Builder;

using Rye.Web.Middleware;

using System;

namespace Rye.Web
{
    public static class SecurityMiddlewareExtensions
    {
        public static IApplicationBuilder UseSecurity(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.UseMiddleware<SecurityMiddleware>();
        }
    }
}
