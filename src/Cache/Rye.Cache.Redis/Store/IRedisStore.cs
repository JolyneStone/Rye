using Rye.Cache.Store;

using System.Threading.Tasks;

namespace Rye.Cache.Redis.Store
{
    public interface IRedisStore: ICacheStore
    {
        /// <summary>
        /// 存缓存中获取值，当 cacheSeconds == -1 时，表示永不过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="cacheSeconds"></param>
        /// <returns></returns>
        T Get<T>(string key, int cacheSeconds = 60);

        /// <summary>
        /// 存缓存中获取值，当 cacheSeconds == -1 时，表示永不过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="cacheSeconds"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key, int cacheSeconds = 60);
    }
}
