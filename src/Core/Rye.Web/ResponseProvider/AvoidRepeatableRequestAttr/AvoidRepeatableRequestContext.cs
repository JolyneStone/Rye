using Microsoft.AspNetCore.Http;

namespace Rye.Web.ResponseProvider.AvoidRepeatableRequestAttr
{
    public class AvoidRepeatableRequestContext
    {
        public AvoidRepeatableRequestContext(HttpContext httpContext, string cacheKey)
        {
            HttpContext = httpContext;
            CacheKey = cacheKey;
        }

        public string CacheKey { get; }

        public HttpContext HttpContext { get; }
    }
}
