using System.Threading.Tasks;

namespace Rye.Cache.Store
{
    public interface IMemoryStore: ICacheStore
    {
        /// <summary>
        /// 从缓存中获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);
        /// <summary>
        /// 从缓存中获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);
    }
}
