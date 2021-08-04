using System;
using System.Threading.Tasks;

namespace Rye.Cache.Store
{
    public interface ICacheStore: IDisposable
    {
        /// <summary>
        /// 设置缓存，当 cacheSeconds == -1 时，表示永不过期
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
        /// <param name="entry"></param>
        /// <param name="data"></param>
        void Set<T>(CacheOptionEntry entry, T data);

        /// <summary>
        /// 异步设置缓存，当 cacheSeconds == -1 时，表示永不过期
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
        /// <param name="entry"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task SetAsync<T>(CacheOptionEntry entry, T data);

        /// <summary>
        /// 从缓存中获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 异步从缓存中获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// 从缓存中获取值，若键不存在，则使用func参数获取值，并添加到缓存中，当 cacheSeconds == -1 时，表示永不过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="cacheSeconds"></param>
        /// <returns></returns>
        T Get<T>(string key, Func<T> func, int cacheSeconds = 60);
        /// <summary>
        /// 从缓存中获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        /// <returns></returns>
        T Get<T>(CacheOptionEntry entry);
        /// <summary>
        /// 从缓存中获取值，若键不存在，则使用func参数获取值，并添加到缓存中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        T Get<T>(CacheOptionEntry entry, Func<T> func);
        /// <summary>
        /// 从缓存中获取值，若键不存在，则使用func参数获取值，并添加到缓存中，当 cacheSeconds == -1 时，表示永不过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="cacheSeconds"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key, Func<T> func, int cacheSeconds = 60);
        /// <summary>
        /// 从缓存中获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(CacheOptionEntry entry);
        /// <summary>
        /// 从缓存中获取值，若键不存在，则使用func参数获取值，并添加到缓存中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(CacheOptionEntry entry, Func<T> func);
        /// <summary>
        /// 从缓存中获取值，若键不存在，则使用func参数获取值，并添加到缓存中，当 cacheSeconds == -1 时，表示永不过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="cacheSeconds"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key, Func<Task<T>> func, int cacheSeconds = 60);
        /// <summary>
        /// 从缓存中获取值，若键不存在，则使用func参数获取值，并添加到缓存中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(CacheOptionEntry entry, Func<Task<T>> func);
        /// <summary>
        /// 删除缓存中指定的键值，如果该键存在的话
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
        /// <summary>
        /// 删除缓存中指定的键值，如果该键存在的话
        /// </summary>
        /// <param name="entry"></param>
        void Remove(CacheOptionEntry entry);
        /// <summary>
        /// 删除缓存中指定的键值，如果该键存在的话
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task RemoveAsync(string key);
        /// <summary>
        /// 删除缓存中指定的键值，如果该键存在的话
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        Task RemoveAsync(CacheOptionEntry entry);

        /// <summary>
        /// 判断缓存中是否存在该键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exists(string key);
        /// <summary>
        /// 判断缓存中是否存在该键值
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        bool Exists(CacheOptionEntry entry);
        /// <summary>
        /// 判断缓存中是否存在该键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key);
        /// <summary>
        /// 判断缓存中是否存在该键值
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(CacheOptionEntry entry);
    }
}
