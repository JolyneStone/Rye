using Microsoft.Extensions.Options;

using Rye.Business.Options;
using Rye.Cache;
using Rye.Cache.Internal;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Rye.Business.Validate
{
    public class VerifyCodeService: IVerifyCodeService
    {
        private const string Separator = "#$#";
        private readonly ICacheService _cacheService;
        private readonly BusinessOptions _options;

        public VerifyCodeService(ICacheService cacheService, IOptions<BusinessOptions> options)
        {
            _cacheService = cacheService;
            _options = options.Value;
        }

        /// <summary>
        /// 校验验证码有效性
        /// </summary>
        /// <param name="code">要校验的验证码</param>
        /// <param name="id">验证码编号</param>
        /// <param name="removeIfSuccess">验证成功时是否移除</param>
        /// <returns></returns>
        public virtual bool CheckCode(string id, string code, bool removeIfSuccess = true)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }

            var entry = CacheEntryCollection.GetVerifyCodeEntry(id);
            bool flag = code.Equals(_cacheService.Get<string>(entry.CacheKey), StringComparison.OrdinalIgnoreCase);
            if (removeIfSuccess && flag)
            {
                _cacheService.Remove(entry.CacheKey);
            }

            return flag;
        }

        /// <summary>
        /// 设置验证码到缓存中
        /// </summary>
        public virtual void SetCode(string code, out string id)
        {
            id = Guid.NewGuid().ToString("N");
            var entry = CacheEntryCollection.GetVerifyCodeEntry(id, _options.VerfiyCodeExpire);
            _cacheService.Set(entry.CacheKey, code, entry.Options);
        }

        /// <summary>
        /// 将图片序列化成字符串
        /// </summary>
        public virtual string GetImageString(Image image, string id)
        {
            Check.NotNull(image, nameof(image));
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                byte[] bytes = ms.ToArray();
                string str = $"data:image/png;base64,{bytes.ToBase64String()}{Separator}{id}";
                return str.ToBase64String();
            }
        }
    }
}
