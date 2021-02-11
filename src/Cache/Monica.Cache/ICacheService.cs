using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Monica.Cache
{
    public interface ICacheService
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheSeconds"></param>
        void Set<T>(string key, T data, int cacheSeconds = 60);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="options"></param>
        void Set<T>(string key, T data, DistributedCacheEntryOptions options);

        /// <summary>
        /// 异步设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheSeconds"></param>
        /// <returns></returns>
        Task SetAsync<T>(string key, T data, int cacheSeconds = 60);

        /// <summary>
        /// 异步设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task SetAsync<T>(string key, T data, DistributedCacheEntryOptions options);

        /// <summary>
        /// 异步设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="options"></param>
        /// <param name="token"></param>
        Task SetAsync<T>(string key, T data, DistributedCacheEntryOptions options, CancellationToken token = default);

        /// <summary>
        /// 从缓存中获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);
        /// <summary>
        /// 存缓存中获取值，若键不存在，则使用func参数获取值，并添加到缓存中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="cacheSeconds"></param>
        /// <returns></returns>
        T Get<T>(string key, Func<IServiceProvider, T> func, int cacheSeconds = 60);
        /// <summary>
        /// 存缓存中获取值，若键不存在，则使用func参数获取值，并添加到缓存中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        T Get<T>(string key, Func<IServiceProvider, T> func, DistributedCacheEntryOptions options);
        /// <summary>
        /// 从缓存中获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key, CancellationToken token = default);
        /// <summary>
        /// 存缓存中获取值，若键不存在，则使用func参数获取值，并添加到缓存中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="cacheSeconds"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key, Func<IServiceProvider, T> func, int cacheSeconds = 60, CancellationToken token = default);
        /// <summary>
        /// 存缓存中获取值，若键不存在，则使用func参数获取值，并添加到缓存中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key, Func<IServiceProvider, T> func, DistributedCacheEntryOptions options, CancellationToken token = default);
        /// <summary>
        /// 存缓存中获取值，若键不存在，则使用func参数获取值，并添加到缓存中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="cacheSeconds"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key, Func<IServiceProvider, Task<T>> func, int cacheSeconds = 60, CancellationToken token = default);
        /// <summary>
        /// 存缓存中获取值，若键不存在，则使用func参数获取值，并添加到缓存中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key, Func<IServiceProvider, Task<T>> func, DistributedCacheEntryOptions options, CancellationToken token = default);
        /// <summary>
        /// 删除缓存中指定的键值，如果该键存在的话
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
        /// <summary>
        /// 删除缓存中指定的键值，如果该键存在的话
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task RemoveAsync(string key, CancellationToken token = default);
        /// <summary>
        /// 刷新缓存中的键值，以重置滑动过期时间
        /// </summary>
        /// <param name="key"></param>
        void Refresh(string key);
        /// <summary>
        /// 刷新缓存中的键值，以重置滑动过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task RefreshAsync(string key, CancellationToken token = default);
        /// <summary>
        /// 判断缓存中是否存在该键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exist(string key);
        /// <summary>
        /// 判断缓存中是否存在该键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> ExistAsync(string key, CancellationToken token = default);
    }
}
