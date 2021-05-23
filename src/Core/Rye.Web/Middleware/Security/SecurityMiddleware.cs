
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

using Rye.Business.Language;
using Rye.Entities.Abstractions;
using Rye.Security;
using Rye.Web.Options;
using Rye.Web.ResponseProvider.Security;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
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
                var appSecret = await appInfoService.GetAppSecretAsync(appKey.ToString());
                if (appSecret.IsNullOrEmpty())
                {
                    responseTempBody = _provider.CreateParameterErrorResponse(new SecurityContext { HttpContext = context });
                    await WriteResponseAsync(context, attr.EncryptResponseBody, securityService, null, null, responseTempBody);
                }

                try
                {
                    if (attr.DecryptRequestBody && !string.Equals(context.Request.Method, "get", StringComparison.InvariantCultureIgnoreCase) &&
                        context.Request.Form != null)//解密
                    {
                        #region 解密请求包体的内容  
                        try
                        {
                            var formCollection = context.Request.Form;
                            var dic = new Dictionary<string, StringValues>();
                            foreach (var key in formCollection.Keys)
                            {
                                var val = formCollection[key];
                                if (string.Equals(key, "param", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    var param = securityService.Decrypt(appKey, appSecret, val.ToString());
                                    var paramDic = param.ToObject<Dictionary<string, JsonElement>>();
                                    if (paramDic != null)
                                    {
                                        foreach (var pair in paramDic)
                                        {
                                            var tempStr = pair.Value.GetRawText();
                                            dic.Add(pair.Key,
                                                new StringValues(tempStr.IsNullOrEmpty() || tempStr.Length<3 ? 
                                                    "" :
                                                    new string(tempStr.AsSpan(1, tempStr.Length - 2))));
                                        }
                                    }
                                }
                                else
                                {
                                    dic.Add(key, val);
                                }
                            }
                            var fc = new FormCollection(dic);
                            context.Request.Form = fc;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.ToString());
                        }
                        #endregion
                    }
                    if (attr.EncryptResponseBody)
                    {
                        #region 返回的内容进行加密处理
                        var originResponse = context.Response.Body;
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

                        context.Response.Body = originResponse;
                        if (!string.IsNullOrEmpty(responseTempBody))
                        {
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
            }
        }

        private async Task WriteResponseAsync(HttpContext context, bool encrypt, ISecurityService securityService, string appKey, string appSecret, string value)
        {
            if (encrypt && !appKey.IsNullOrEmpty() && !appSecret.IsNullOrEmpty())
            {
                value = securityService.Encrypt(appKey, appSecret, value); //加密再给body
            }
            context.Response.Headers["Content-length"] = value.Length.ToString();
            Stream responseOrigin = context.Response.Body;
            await responseOrigin.FlushAsync();
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
