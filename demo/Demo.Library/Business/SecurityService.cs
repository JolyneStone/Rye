using Demo.Core.Common;
using Demo.DataAccess;

using Rye.Cache;
using Rye.Security;

using System;
using System.Text;

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

        public byte[] Decrypt(int appId, byte[] bytes)
        {
            var cacheKey = string.Format(MemoryCacheKeys.AppInfoById, appId);
            var appInfo = MemoryCacheManager.Get<AppInfo>(cacheKey);
            if (appInfo == null)
            {
                appInfo = _appInfoDataAccess.GetModelWithCache(appId);
                if (appInfo == null)
                {
                    throw new InvalidOperationException("Decryption failed");
                }

                MemoryCacheManager.Set(cacheKey, appInfo, MemoryCacheKeys.AppInfoById_TimeOut);
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
            var cacheKey = string.Format(MemoryCacheKeys.AppInfoById, appId);
            var appInfo = MemoryCacheManager.Get<AppInfo>(cacheKey);
            if (appInfo == null)
            {
                appInfo = _appInfoDataAccess.GetModelWithCache(appId);
                if (appInfo == null)
                {
                    throw new InvalidOperationException("Decryption failed");
                }

                MemoryCacheManager.Set(cacheKey, appInfo, MemoryCacheKeys.AppInfoById_TimeOut);
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
