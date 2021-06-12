using Demo.Common;
using Demo.DataAccess;

using Rye.Cache;
using Rye.Cache.Store;
using Rye.Security;

using System;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Business
{
    public class SecurityService : ISecurityService
    {
        private readonly IAppInfo _appInfoDataAccess;

        public SecurityService(IAppInfo appInfoDataAccess)
        {
            _appInfoDataAccess = appInfoDataAccess;
        }

        public string Decrypt(int appId, string value)
        {
            return Encoding.UTF8.GetString(Decrypt(appId, Convert.FromBase64String(value)));
        }

        public async Task<string> DecryptAsync(int appId, string value)
        {
            return Encoding.UTF8.GetString(await DecryptAsync(appId, Convert.FromBase64String(value)));
        }

        public async Task<byte[]> DecryptAsync(int appId, byte[] bytes)
        {
            var appInfo = await _appInfoDataAccess.GetModelWithCacheAsync(appId);
            if (appInfo == null)
            {
                throw new InvalidOperationException("Decryption failed");
            }

            return AesManager.Decrypt(bytes, appInfo.AppKey, appInfo.AppSecret);
        }

        public byte[] Decrypt(int appId, byte[] bytes)
        {
            var appInfo = _appInfoDataAccess.GetModelWithCache(appId);
            if (appInfo == null)
            {
                throw new InvalidOperationException("Decryption failed");
            }

            return AesManager.Decrypt(bytes, appInfo.AppKey, appInfo.AppSecret);
        }

        public string Decrypt(string key, string iv, string value)
        {
            return AesManager.Decrypt(value, key, iv, Encoding.UTF8);
        }

        public byte[] Decrypt(string key, string iv, byte[] bytes)
        {
            return AesManager.Decrypt(bytes, key, iv, Encoding.UTF8);
        }

        public string Encrypt(int appId, string value)
        {
            return Convert.ToBase64String(Encrypt(appId, Encoding.UTF8.GetBytes(value)));
        }


        public byte[] Encrypt(int appId, byte[] bytes)
        {
            var appInfo = _appInfoDataAccess.GetModelWithCache(appId);
            if (appInfo == null)
            {
                throw new InvalidOperationException("Decryption failed");
            }

            return Encrypt(appInfo.AppKey, appInfo.AppSecret, bytes);
        }

        public async Task<string> EncryptAsync(int appId, string value)
        {
            return Convert.ToBase64String(await EncryptAsync(appId, Encoding.UTF8.GetBytes(value)));
        }

        public async Task<byte[]> EncryptAsync(int appId, byte[] bytes)
        {
            var appInfo = await _appInfoDataAccess.GetModelWithCacheAsync(appId);
            if (appInfo == null)
            {
                throw new InvalidOperationException("Decryption failed");
            }

            return Encrypt(appInfo.AppKey, appInfo.AppSecret, bytes);
        }

        public string Encrypt(string key, string iv, string value)
        {
            return AesManager.Encrypt(value, key, iv, Encoding.UTF8);
        }

        public byte[] Encrypt(string key, string iv, byte[] bytes)
        {
            return AesManager.Encrypt(bytes, key, iv, Encoding.UTF8);
        }
    }
}
