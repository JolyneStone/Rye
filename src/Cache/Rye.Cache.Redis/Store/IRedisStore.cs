using CSRedis;

using Rye.Cache.Store;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rye.Cache.Redis.Store
{
    public interface IRedisStore: ICacheStore
    {
        /// <summary>
        /// 表示是否只读
        /// </summary>
        bool ReadOnly { get; }

        /// <summary>
        /// 获取Redis客户端
        /// </summary>
        CSRedisClient Client { get; }

        #region 哈希表

        /// <summary>
        /// 计算哈希表长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long HashCount(string key);

        /// <summary>
        /// 异步计算哈希表长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> HashCountAsync(string key);

        /// <summary>
        /// 从哈希表中获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        T GetFromHash<T>(string key, string field);

        /// <summary>
        /// 异步从哈希表中获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        Task<T> GetFromHashAsync<T>(string key, string field);

        /// <summary>
        /// 从哈希表中获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        T[] GetFromHash<T>(string key, params string[] fields);

        /// <summary>
        /// 异步从哈希表中获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        Task<T[]> GetFromHashAsync<T>(string key, params string[] fields);

        /// <summary>
        /// 将哈希表 key 中的字段 field 的值设为 value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        void SetToHash(string key, string field, object value);

        /// <summary>
        /// 异步将字典 dict 里的值设置到哈希表 key 中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        Task SetDictToHashAsync(string key, Dictionary<string, object> dict);

        /// <summary>
        /// 将将字典 dict 里的值设置到哈希表 key 中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        void SetDictToHash(string key, Dictionary<string, object> dict);

        /// <summary>
        /// 从哈希表中获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Dictionary<string, T> GetAllFromHash<T>(string key);

        /// <summary>
        /// 异步从哈希表中获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<Dictionary<string, T>> GetAllFromHashAsync<T>(string key);

        /// <summary>
        /// 异步将哈希表 key 中的字段 field 的值设为 value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetToHashAsync(string key, string field, object value);

        #endregion

        #region 列表

        /// <summary>
        /// 将values 的值依次入队到列表key中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        long Enqueue<T>(string key, params T[] values);

        /// <summary>
        /// 异步将 values 的值依次入队到列表key中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        Task<long> EnqueueAsync<T>(string key, params T[] values);

        /// <summary>
        /// 将列表 key 中的最后一个元素出队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Dequeue<T>(string key);

        /// <summary>
        /// 异步将列表 key 中的最后一个元素出队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> DequeueAsync<T>(string key);

        /// <summary>
        /// 计算列表长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long ListCount(string key);

        /// <summary>
        /// 异步计算列表长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> ListCountAsync(string key);

        /// <summary>
        /// 将 values 的值压入列表 key 中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        long Push<T>(string key, params T[] values);

        /// <summary>
        /// 异步将 values 的值压入列表 key 中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        Task<long> PushAsync<T>(string key, params T[] values);

        /// <summary>
        /// 将列表 key 的第一个元素弹出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Pop<T>(string key);

        /// <summary>
        /// 异步将列表 key 的第一个元素弹出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> PopAsync<T>(string key);

        #endregion

        #region 集合

        /// <summary>
        /// 添加 values 到集合 key 中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        long AddToSet<T>(string key, params T[] values);

        /// <summary>
        /// 异步添加 values 到集合 key 中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        Task<long> AddToSetAsync<T>(string key, params T[] values);

        /// <summary>
        /// 获取集合 key 中的所有元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T[] GetAllSet<T>(string key);

        /// <summary>
        /// 异步获取集合 key 中的所有元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T[]> GetAllSetAsync<T>(string key);

        /// <summary>
        /// 随机弹出集合的一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T PopSet<T>(string key);

        /// <summary>
        /// 异步随机弹出集合的一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> PopSetAsync<T>(string key);

        /// <summary>
        /// 随机获取集合的一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetRandSet<T>(string key);

        /// <summary>
        /// 异步随机获取集合的一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetRandSetAsync<T>(string key);

        /// <summary>
        /// 删除集合 key 中的元素 values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        long RemoveSet<T>(string key, params T[] values);

        /// <summary>
        /// 异步删除集合 key 中的元素 values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        Task<long> RemoveSetAsync<T>(string key, params T[] values);

        /// <summary>
        /// 获取多个集合的差集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        T[] DifferencesFromSet<T>(params string[] keys);

        /// <summary>
        /// 异步获取多个集合的差集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<T[]> DifferencesFromSetAsync<T>(params string[] keys);

        /// <summary>
        /// 获取多个集合的交集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        T[] IntersectFromSet<T>(params string[] keys);

        /// <summary>
        /// 异步获取多个集合的交集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<T[]> IntersectFromSetAsync<T>(params string[] keys);

        /// <summary>
        /// 获取多个集合的并集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        T[] UnionFromSet<T>(params string[] keys);

        /// <summary>
        /// 异步获取多个集合的并集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<T[]> UnionFromSetAsync<T>(params string[] keys);

        /// <summary>
        /// 将元素 value 从集合 source 移动到集合 destination
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool MoveSet(string source, string destination, object value);

        /// <summary>
        /// 异步将元素 value 从集合 source 移动到集合 destination
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> MoveSetAsync(string source, string destination, object value);

        /// <summary>
        /// 计算集合长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long SetCount(string key);

        /// <summary>
        /// 异步计算集合长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> SetCountAsync(string key);

        #endregion

        #region 有序集合

        /// <summary>
        /// 添加成员到有序集合中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="score"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        long AddToSortedSet(string key, decimal score, object member);

        /// <summary>
        /// 异步添加成员到有序集合中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="score"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        Task<long> AddToSortedSetAsync(string key, decimal score, object member);

        /// <summary>
        /// 添加成员到有序集合中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        long AddToSortedSet(string key, params (decimal, object)[] members);

        /// <summary>
        /// 异步添加成员到有序集合中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        Task<long> AddToSortedSetAsync(string key, params (decimal, object)[] members);

        /// <summary>
        /// 计算有序集合长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long SortedSetCount(string key);

        /// <summary>
        /// 异步计算有序集合长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> SortedSetCountAsync(string key);

        /// <summary>
        /// 计算有序集合分数区间内的成员个数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        long SortedSetCount(string key, decimal min, decimal max);

        /// <summary>
        /// 异步计算有序集合分数区间内的成员个数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        Task<long> SortedSetCountAsync(string key, decimal min, decimal max);

        /// <summary>
        /// 获取成员在有序集合内的排名
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        long? GetRankFormSortedSet(string key, object member);

        /// <summary>
        /// 异步获取成员在有序集合内的排名
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        Task<long?> GetRankFormSortedSetAsync(string key, object member);

        /// <summary>
        /// 获取成员在有序集合内的分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        decimal? GetScoreFormSortedSet(string key, object member);

        /// <summary>
        /// 异步获取成员在有序集合内的分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        Task<decimal?> GetScoreFormSortedSetAsync(string key, object member);

        /// <summary>
        /// 对有序集合中对指定成员的分数加上score
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        decimal IncrementScore(string key, object member, decimal score);

        /// <summary>
        /// 异步对有序集合中对指定成员的分数加上score
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        Task<decimal> IncrementScoreAsync(string key, object member, decimal score);

        /// <summary>
        /// 获取有序集合排名区间内的成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="isDesending"></param>
        /// <returns></returns>
        T[] GetRangeFormSortedSet<T>(string key, long start, long stop, bool isDesending = false);

        /// <summary>
        /// 异步获取有序集合排名区间内的成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="isDesending"></param>
        /// <returns></returns>
        Task<T[]> GetRangeFormSortedSetAsync<T>(string key, long start, long stop, bool isDesending = false);

        /// <summary>
        /// 获取有序集合分数区间内的成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <param name="isDesending"></param>
        /// <returns></returns>
        T[] GetRangeFormSortedSet<T>(string key, decimal min, decimal max, long? count = null, long offset = 0, bool isDesending = false);

        /// <summary>
        /// 获取有序集合分数区间内的成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <param name="isDesending"></param>
        /// <returns></returns>
        Task<T[]> GetRangeFormSortedSetAsync<T>(string key, decimal min, decimal max, long? count = null, long offset = 0, bool isDesending = false);

        /// <summary>
        /// 获取有序集合排名区间内的成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="isDesending"></param>
        /// <returns></returns>
        (T, decimal)[] GetRangeWithScoreFormSortedSet<T>(string key, long start, long stop, bool isDesending = false);

        /// <summary>
        /// 异步获取有序集合排名区间内的成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="isDesending"></param>
        /// <returns></returns>
        Task<(T, decimal)[]> GetRangeWithScoreFormSortedSetAsync<T>(string key, long start, long stop, bool isDesending = false);

        /// <summary>
        /// 获取有序集合分数区间内的成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <param name="isDesending"></param>
        /// <returns></returns>
        (T, decimal)[] GetRangeWithScoreFormSortedSet<T>(string key, decimal min, decimal max, long? count = null, long offset = 0, bool isDesending = false);

        /// <summary>
        /// 获取有序集合分数区间内的成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <param name="isDesending"></param>
        /// <returns></returns>
        Task<(T, decimal)[]> GetRangeWithScoreFormSortedSetAsync<T>(string key, decimal min, decimal max, long? count = null, long offset = 0, bool isDesending = false);


        /// <summary>
        /// 移除有序集合中的指定成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        long RemoveSortedSet<T>(string key, params T[] members);

        /// <summary>
        /// 异步移除有序集合中的指定成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        Task<long> RemoveSortedSetAsync<T>(string key, params T[] members);

        /// <summary>
        /// 移除有序集合排名区间内的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        long RemoveRangeSortedSet(string key, int start, int stop);

        /// <summary>
        /// 异步移除有序集合排名区间内的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        Task<long> RemoveRangeSortedSetAsync(string key, int start, int stop);

        /// <summary>
        /// 移除有序集合分数区间内的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        long RemoveRangeSortedSet(string key, decimal min, decimal max);

        /// <summary>
        /// 异步移除有序集合分数区间内的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        Task<long> RemoveRangeSortedSetAsync(string key, decimal min, decimal max);

        #endregion

        /// <summary>
        /// 设置key过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        bool Expire(string key, TimeSpan expire);

        /// <summary>
        /// 设置key过期时间，单位：秒
        /// </summary>
        /// <param name="key"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        bool Expire(string key, int seconds);

        /// <summary>
        /// 设置key过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        Task<bool> ExpireAsync(string key, TimeSpan expire);

        /// <summary>
        /// 设置key过期时间，单位：秒
        /// </summary>
        /// <param name="key"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        Task<bool> ExpireAsync(string key, int seconds);

        /// <summary>
        /// 设置key在某一时间过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        bool ExpireAt(string key, DateTime expire);

        /// <summary>
        /// 异步设置key在某一时间过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        Task<bool> ExpireAtAsync(string key, DateTime expire);

        /// <summary>
        /// 使得key中存储的数值增加increment，并返回增加后的数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        long Increment(string key, long increment = 1);

        /// <summary>
        /// 异步使得key中存储的数值increment，并返回增加后的数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        Task<long> IncrementAsync(string key, long increment = 1);
    }
}
