
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

using Rye;
using Rye.Entities.Abstractions;
using Rye.Language;
using Rye.Security;
using Rye.Web.Attribute;
using Rye.Web.Options;
using Rye.Web.ResponseProvider.Security;

using System;
using System.IO;
using System.Threading.Tasks;

namespace Rye.Web.Middleware
{
    public class SecurityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SecurityMiddleware> _logger;
        private readonly ISecurityResponseProvider _provider;

        public SecurityMiddleware(RequestDelegate next,
            ILogger<SecurityMiddleware> logger,
            IOptions<RyeWebOptions> options)
        {
            _next = next;
            _logger = logger;
            _provider = options.Value.Security.Provider;
        }

        public async Task Invoke(HttpContext context)
        {
            var attr = context.Features.Get<IEndpointFeature>()?.Endpoint.Metadata.GetMetadata<SecurityAttribute>();
            if (attr == null)
            {
                await _next.Invoke(context).ConfigureAwait(false);
                return;
            }

            var services = context.RequestServices;
            var securityService = services.GetRequiredService<ISecurityService>();

            var responseTempBody = string.Empty;
            string lang = LangCode.ZHCN;
            if (context.Request.Query.TryGetValue("lang", out var langVal))
            {
                lang = langVal.ToString();
            }
            if (!context.Request.Query.TryGetValue("appKey", out var appKey) || StringValues.IsNullOrEmpty(appKey))
            {
                responseTempBody = _provider.CreateParameterErrorResponse(new SecurityContext { HttpContext = context });
                await WriteResponseAsync(context, attr.EncryptResponseBody, securityService, null, null, responseTempBody);
                return;
            }
            else
            {
                var appInfoService = services.GetRequiredService<IAppInfoService>();
                var appSecret = appInfoService.GetAppSecret(appKey.ToString());
                if (appSecret.IsNullOrEmpty())
                {
                    responseTempBody = _provider.CreateParameterErrorResponse(new SecurityContext { HttpContext = context });
                    await WriteResponseAsync(context, attr.EncryptResponseBody, securityService, null, null, responseTempBody);
                }

                Stream requestOrigin = context.Request.Body;
                try
                {
                    if (string.Equals(context.Request.Method, "get", StringComparison.InvariantCultureIgnoreCase) && attr.DecryptRequestBody)//解密
                    {
                        #region 解密请求包体的内容  
                        string requestStr;
                        if (requestOrigin.CanRead)
                        {
                            try
                            {
                                using (var reader = new StreamReader(requestOrigin))
                                {
                                    requestStr = await reader.ReadToEndAsync();
                                }
                                if (!requestStr.IsNullOrEmpty())
                                {
                                    requestStr = requestStr.ToObject<SecurityBody>().Param;
                                    var requestBytes = securityService.Decrypt(appKey, appSecret, Convert.FromBase64String(requestStr));
                                    MemoryStream inputStream = new MemoryStream(requestBytes);
                                    inputStream.Position = 0;
                                    context.Request.Body = inputStream;
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.ToString());
                            }
                        }
                        #endregion
                    }
                    if (attr.EncryptResponseBody)
                    {
                        #region 返回的内容进行加密处理
                        using (MemoryStream newResponse = new MemoryStream())
                        {
                            context.Response.Body = newResponse;

                            await _next(context).ConfigureAwait(false); //执行action

                            using (StreamReader streamReader = new StreamReader(newResponse))
                            {
                                newResponse.Position = 0;
                                responseTempBody = await streamReader.ReadToEndAsync(); //action返回的数据
                            }
                        }
                        if (!string.IsNullOrEmpty(responseTempBody))
                        {
                            responseTempBody = responseTempBody.ToObject<SecurityBody>().Param;
                            await WriteResponseAsync(context, attr.EncryptResponseBody, securityService, appKey, appSecret, responseTempBody);
                        }

                        #endregion
                    }
                    else
                    {
                        await _next(context).ConfigureAwait(false); //执行action
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }
                finally
                {
                    requestOrigin?.Dispose();
                }
            }
        }

        private async Task WriteResponseAsync(HttpContext context, bool encrypt, ISecurityService securityService, string appKey, string appSecret, string value)
        {
            Stream responseOrigin = context.Response.Body;
            if (encrypt && !appKey.IsNullOrEmpty() && !appSecret.IsNullOrEmpty())
            {
                value = securityService.Encrypt(appKey, appSecret, value); //加密再给body
            }
            context.Response.Headers["Content-length"] = value.Length.ToString();
            await responseOrigin.FlushAsync();
            responseOrigin.Position = 0;
            using (StreamWriter streamWriter = new StreamWriter(responseOrigin))
            {
                await streamWriter.WriteAsync(value);
            }
        }

        private class SecurityBody
        {
            public string Param { get; set; }
        }
    }
}
