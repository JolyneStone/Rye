using Microsoft.Extensions.Options;

using Rye.Business.Options;
using Rye.Cache;
using Rye.Cache.Store;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Rye.Business.Validate
{
    public class VerifyCodeService: IVerifyCodeService
    {
        private const string Separator = "#$#";
        private readonly ICacheStore _store;
        private readonly BusinessOptions _options;

        public VerifyCodeService(ICacheStore store, IOptions<BusinessOptions> options)
        {
            _store = store;
            _options = options.Value;
        }

        /// <summary>
        /// 校验验证码有效性
        /// </summary>
        /// <param name="code">要校验的验证码</param>
        /// <param name="id">验证码编号</param>
        /// <param name="removeIfSuccess">验证成功时是否移除</param>
        /// <returns></returns>
        public virtual async Task<bool> CheckCodeAsync(string id, string code, bool removeIfSuccess = true)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }

            var entry = CacheEntryCollection.GetVerifyCodeEntry(id);
            var validCode = _store.Get<string>(entry);
            bool flag = code.Equals(validCode, StringComparison.InvariantCultureIgnoreCase);
            if (removeIfSuccess && flag)
            {
                await _store.RemoveAsync(entry.Key);
            }

            return flag;
        }

        /// <summary>
        /// 设置验证码到缓存中
        /// </summary>
        public virtual async Task<string> SetCodeAsync(string code)
        {
            var id = Guid.NewGuid().ToString("N");
            var entry = CacheEntryCollection.GetVerifyCodeEntry(id, _options.VerfiyCodeExpire);
            await _store.SetAsync(entry, code);
            return id;
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
                return str;
            }
        }
    }
}
