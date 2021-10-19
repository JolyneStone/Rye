using CSRedis;

using Microsoft.Extensions.Options;

using Rye.Cache.Redis.Builder;
using Rye.Cache.Redis.Options;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rye.Cache.Redis.Store
{
    public class RedisStore : IRedisStore
    {
        private readonly CSRedisClient _redisClient;

        public CSRedisClient Client { get => _redisClient; }

        public bool ReadOnly { get; }

        public RedisStore(IOptions<RedisOptions> options)
        {
            var redisOptions = options.Value;
            ReadOnly = redisOptions.ReadOnly;
            _redisClient = new CSRedisClient(new RedisConnectionBuilder().BuildConnectionString(redisOptions), redisOptions.Sentinels);
        }

        public bool Exists(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            return _redisClient.Exists(key);
        }

        public bool Exists(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));

            return _redisClient.Exists(entry.Key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            return await _redisClient.ExistsAsync(key);
        }

        public async Task<bool> ExistsAsync(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));

            return await _redisClient.ExistsAsync(entry.Key);
        }

        public T Get<T>(string key, Func<T> func, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            Check.NotNull(func, nameof(func));

            var data = _redisClient.Get<T>(key);

            if (Equals(data, default(T)))
            {
                data = func();
                if (!Equals(data, default(T)))
                    return data;
            }

            _redisClient.Set(key, data, cacheSeconds);
            return data;
        }

        public T Get<T>(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            return _redisClient.Get<T>(key);
        }

        public T Get<T>(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            return _redisClient.Get<T>(entry.Key);
        }

        public T Get<T>(CacheOptionEntry entry, Func<T> func)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            Check.NotNull(func, nameof(func));

            var data = _redisClient.Get<T>(entry.Key);
            if (!Equals(data, default(T)))
            {
                data = func();
                if (!Equals(data, default(T)))
                    return data;
            }

            _redisClient.Set(entry.Key, data, entry.ConvertToTimeSpanRelativeToNow());
            return data;
        }

        public Task<T> GetAsync<T>(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            return _redisClient.GetAsync<T>(key);
        }

        public async Task<T> GetAsync<T>(string key, Func<T> func, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            Check.NotNull(func, nameof(func));

            var data = await _redisClient.GetAsync<T>(key);
            if (!Equals(data, default(T)))
            {
                data = func();
                if (!Equals(data, default(T)))
                    return data;
            }

            await _redisClient.SetAsync(key, data, cacheSeconds);
            return data;
        }

        public async Task<T> GetAsync<T>(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            return await _redisClient.GetAsync<T>(entry.Key);
        }

        public async Task<T> GetAsync<T>(CacheOptionEntry entry, Func<T> func)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            Check.NotNull(func, nameof(func));

            var data = await _redisClient.GetAsync<T>(entry.Key);
            if (Equals(data, default(T)))
            {
                data = func();
                if (!Equals(data, default(T)))
                    return data;
            }

            await _redisClient.SetAsync(entry.Key, data, entry.ConvertToTimeSpanRelativeToNow());
            return data;
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> func, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            Check.NotNull(func, nameof(func));
            T data = await _redisClient.GetAsync<T>(key);
            if (!Equals(data, default(T)))
            {
                data = await func();
                if (!Equals(data, default(T)))
                    return data;
            }

            await _redisClient.SetAsync(key, data, cacheSeconds);
            return data;
        }

        public async Task<T> GetAsync<T>(CacheOptionEntry entry, Func<Task<T>> func)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            Check.NotNull(func, nameof(func));

            var data = await _redisClient.GetAsync<T>(entry.Key);
            if (!Equals(data, default(T)))
            {
                data = await func();
                if (!Equals(data, default(T)))
                    return data;
            }

            await _redisClient.SetAsync(entry.Key, data, entry.ConvertToTimeSpanRelativeToNow());
            return data;
        }

        public void Remove(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            _redisClient.Del(key);
        }

        public void Remove(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            _redisClient.Del(entry.Key);
        }

        public async Task RemoveAsync(string key)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            await _redisClient.DelAsync(key);
        }

        public async Task RemoveAsync(CacheOptionEntry entry)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            await _redisClient.DelAsync(entry.Key);
        }

        public void Set<T>(string key, T data, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            _redisClient.Set(key, data, cacheSeconds);
        }

        public void Set<T>(CacheOptionEntry entry, T data)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            _redisClient.Set(entry.Key, data);
        }

        public async Task SetAsync<T>(string key, T data, int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            await _redisClient.SetAsync(key, data, cacheSeconds);
        }

        public async Task SetAsync<T>(CacheOptionEntry entry, T data)
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNullOrEmpty(entry.Key, nameof(entry.Key));
            await _redisClient.SetAsync(entry.Key, data, entry.ConvertToTimeSpanRelativeToNow());
        }

        #region 哈希表

        public long HashCount(string key)
        {
            return _redisClient.HLen(key);
        }

        public Task<long> HashCountAsync(string key)
        {
            return _redisClient.HLenAsync(key);
        }

        public T GetFromHash<T>(string key, string field)
        {
            return _redisClient.HGet<T>(key, field);
        }

        public Task<T> GetFromHashAsync<T>(string key, string field)
        {
            return _redisClient.HGetAsync<T>(key, field);
        }

        public void SetToHash(string key, string field, object value)
        {
            _redisClient.HSet(key, field, value);
        }

        public Task SetToHashAsync(string key, string field, object value)
        {
            return _redisClient.HSetAsync(key, field, value);
        }

        public async Task SetDictToHashAsync(string key, Dictionary<string, object> dict)
        {
            if (dict == null || dict.Count <= 0)
                return;

            object[] keyvalues = new object[dict.Count * 2];
            var i = 0;
            foreach (var pair in dict)
            {
                keyvalues[i] = pair.Key;
                keyvalues[i + 1] = pair.Value;
                i += 2;
            }

            await _redisClient.HMSetAsync(key, keyvalues);
        }

        /// <summary>
        /// 将将字典 dict 里的值设置到哈希表 key 中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        public void SetDictToHash(string key, Dictionary<string, object> dict)
        {
            if (dict == null || dict.Count <= 0)
                return;

            object[] keyvalues = new object[dict.Count * 2];
            var i = 0;
            foreach (var pair in dict)
            {
                keyvalues[i] = pair.Key;
                keyvalues[i + 1] = pair.Value;
                i += 2;
            }

            _redisClient.HMSet(key, keyvalues);
        }

        /// <summary>
        /// 从哈希表中获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, T> GetAllFromHash<T>(string key)
        {
            return _redisClient.HGetAll<T>(key);
        }

        /// <summary>
        /// 异步从哈希表中获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<Dictionary<string, T>> GetAllFromHashAsync<T>(string key)
        {
            return _redisClient.HGetAllAsync<T>(key);
        }

        #endregion

        #region 列表
        public long Enqueue<T>(string key, params T[] values)
        {
            return _redisClient.LPush(key, values);
        }

        public Task<long> EnqueueAsync<T>(string key, params T[] values)
        {
            return _redisClient.LPushAsync(key, values);
        }

        public T Dequeue<T>(string key)
        {
            return _redisClient.RPop<T>(key);
        }

        public Task<T> DequeueAsync<T>(string key)
        {
            return _redisClient.RPopAsync<T>(key);
        }

        public long ListCount(string key)
        {
            return _redisClient.LLen(key);
        }

        public Task<long> ListCountAsync(string key)
        {
            return _redisClient.LLenAsync(key);
        }

        public long Push<T>(string key, params T[] values)
        {
            return _redisClient.LPush(key, values);
        }

        public Task<long> PushAsync<T>(string key, params T[] values)
        {
            return _redisClient.LPushAsync(key, values);
        }
        public T Pop<T>(string key)
        {
            return _redisClient.LPop<T>(key);
        }

        public Task<T> PopAsync<T>(string key)
        {
            return _redisClient.LPopAsync<T>(key);
        }

        #endregion

        #region 集合

        public long AddToSet<T>(string key, params T[] values)
        {
            return _redisClient.SAdd(key, values);
        }

        public Task<long> AddToSetAsync<T>(string key, params T[] values)
        {
            return _redisClient.SAddAsync(key, values);
        }

        public T[] GetAllSet<T>(string key)
        {
            return _redisClient.SMembers<T>(key);
        }

        public Task<T[]> GetAllSetAsync<T>(string key)
        {
            return _redisClient.SMembersAsync<T>(key);
        }

        public T PopSet<T>(string key)
        {
            return _redisClient.SPop<T>(key);
        }

        public Task<T> PopSetAsync<T>(string key)
        {
            return _redisClient.SPopAsync<T>(key);
        }

        public T GetRandSet<T>(string key)
        {
            return _redisClient.SRandMember<T>(key);
        }

        public Task<T> GetRandSetAsync<T>(string key)
        {
            return _redisClient.SRandMemberAsync<T>(key);
        }

        public long RemoveSet<T>(string key, params T[] values)
        {
            return _redisClient.SRem(key, values);
        }

        public Task<long> RemoveSetAsync<T>(string key, params T[] values)
        {
            return _redisClient.SRemAsync(key, values);
        }

        public T[] DifferencesFromSet<T>(params string[] keys)
        {
            return _redisClient.SDiff<T>(keys);
        }

        public Task<T[]> DifferencesFromSetAsync<T>(params string[] keys)
        {
            return _redisClient.SDiffAsync<T>(keys);
        }

        public T[] IntersectFromSet<T>(params string[] keys)
        {
            return _redisClient.SInter<T>(keys);
        }

        public Task<T[]> IntersectFromSetAsync<T>(params string[] keys)
        {
            return _redisClient.SInterAsync<T>(keys);
        }

        public T[] UnionFromSet<T>(params string[] keys)
        {
            return _redisClient.SUnion<T>(keys);
        }

        public Task<T[]> UnionFromSetAsync<T>(params string[] keys)
        {
            return _redisClient.SUnionAsync<T>(keys);
        }

        public bool MoveSet(string source, string destination, object value)
        {
            return _redisClient.SMove(source, destination, value);
        }

        public Task<bool> MoveSetAsync(string source, string destination, object value)
        {
            return _redisClient.SMoveAsync(source, destination, value);
        }

        public long SetCount(string key)
        {
            return _redisClient.SCard(key);
        }

        public Task<long> SetCountAsync(string key)
        {
            return _redisClient.SCardAsync(key);
        }

        #endregion

        #region 有序集合

        public long AddToSortedSet(string key, decimal score, object member)
        {
            return _redisClient.ZAdd(key, (score, member));
        }

        public Task<long> AddToSortedSetAsync(string key, decimal score, object member)
        {
            return _redisClient.ZAddAsync(key, (score, member));
        }

        public long AddToSortedSet(string key, params (decimal, object)[] members)
        {
            return _redisClient.ZAdd(key, members);
        }

        public Task<long> AddToSortedSetAsync(string key, params (decimal, object)[] members)
        {
            return _redisClient.ZAddAsync(key, members);
        }

        public long SortedSetCount(string key)
        {
            return _redisClient.ZCard(key);
        }

        public Task<long> SortedSetCountAsync(string key)
        {
            return _redisClient.ZCardAsync(key);
        }

        public long SortedSetCount(string key, decimal min, decimal max)
        {
            return _redisClient.ZCount(key, min, max);
        }

        public Task<long> SortedSetCountAsync(string key, decimal min, decimal max)
        {
            return _redisClient.ZCountAsync(key, min, max);
        }

        public long? GetRankFormSortedSet(string key, object member)
        {
            return _redisClient.ZRank(key, member);
        }

        public Task<long?> GetRankFormSortedSetAsync(string key, object member)
        {
            return _redisClient.ZRankAsync(key, member);
        }

        public decimal? GetScoreFormSortedSet(string key, object member)
        {
            return _redisClient.ZScore(key, member);
        }

        public Task<decimal?> GetScoreFormSortedSetAsync(string key, object member)
        {
            return _redisClient.ZScoreAsync(key, member);
        }

        public decimal IncrementScore(string key, object member, decimal score)
        {
            return _redisClient.ZIncrBy(key, member, score);
        }

        public Task<decimal> IncrementScoreAsync(string key, object member, decimal score)
        {
            return _redisClient.ZIncrByAsync(key, member.ToString(), score);
        }

        public T[] GetRangeFormSortedSet<T>(string key, long start, long stop, bool isDesending = false)
        {
            return isDesending ? _redisClient.ZRevRange<T>(key, start, stop) : _redisClient.ZRange<T>(key, start, stop);
        }

        public Task<T[]> GetRangeFormSortedSetAsync<T>(string key, long start, long stop, bool isDesending = false)
        {
            return isDesending ? _redisClient.ZRevRangeAsync<T>(key, start, stop) : _redisClient.ZRangeAsync<T>(key, start, stop);
        }

        public T[] GetRangeFormSortedSet<T>(string key, decimal min, decimal max, long? count = null, long offset = 0, bool isDesending = false)
        {
            return isDesending ? _redisClient.ZRevRangeByScore<T>(key, min, max, count, offset) : _redisClient.ZRangeByScore<T>(key, min, max, count, offset);
        }

        public Task<T[]> GetRangeFormSortedSetAsync<T>(string key, decimal min, decimal max, long? count = null, long offset = 0, bool isDesending = false)
        {
            return isDesending ? _redisClient.ZRevRangeByScoreAsync<T>(key, min, max, count, offset) : _redisClient.ZRangeByScoreAsync<T>(key, min, max, count, offset);
        }

        public (T, decimal)[] GetRangeWithScoreFormSortedSet<T>(string key, long start, long stop, bool isDesending = false)
        {
            return isDesending ? _redisClient.ZRevRangeWithScores<T>(key, start, stop) : _redisClient.ZRangeWithScores<T>(key, start, stop);
        }

        public Task<(T, decimal)[]> GetRangeWithScoreFormSortedSetAsync<T>(string key, long start, long stop, bool isDesending = false)
        {
            return isDesending ? _redisClient.ZRevRangeWithScoresAsync<T>(key, start, stop) : _redisClient.ZRangeWithScoresAsync<T>(key, start, stop);
        }

        public (T, decimal)[] GetRangeWithScoreFormSortedSet<T>(string key, decimal min, decimal max, long? count = null, long offset = 0, bool isDesending = false)
        {
            return isDesending ? _redisClient.ZRevRangeByScoreWithScores<T>(key, min, max, count, offset) : _redisClient.ZRangeByScoreWithScores<T>(key, min, max, count, offset);
        }

        public Task<(T, decimal)[]> GetRangeWithScoreFormSortedSetAsync<T>(string key, decimal min, decimal max, long? count = null, long offset = 0, bool isDesending = false)
        {
            return isDesending ? _redisClient.ZRevRangeByScoreWithScoresAsync<T>(key, min, max, count, offset) : _redisClient.ZRangeByScoreWithScoresAsync<T>(key, min, max, count, offset);
        }


        public long RemoveSortedSet<T>(string key, params T[] members)
        {
            return _redisClient.ZRem(key, members);
        }

        public Task<long> RemoveSortedSetAsync<T>(string key, params T[] members)
        {
            return _redisClient.ZRemAsync(key, members);
        }

        public long RemoveRangeSortedSet(string key, int start, int stop)
        {
            return _redisClient.ZRemRangeByRank(key, start, stop);
        }

        public Task<long> RemoveRangeSortedSetAsync(string key, int start, int stop)
        {
            return _redisClient.ZRemRangeByRankAsync(key, start, stop);
        }

        public long RemoveRangeSortedSet(string key, decimal min, decimal max)
        {
            return _redisClient.ZRemRangeByScore(key, min, max);
        }

        public Task<long> RemoveRangeSortedSetAsync(string key, decimal min, decimal max)
        {
            return _redisClient.ZRemRangeByScoreAsync(key, min, max);
        }

        #endregion

        public bool Expire(string key, TimeSpan expire)
        {
            return _redisClient.Expire(key, expire);
        }
        public bool Expire(string key, int seconds)
        {
            return _redisClient.Expire(key, seconds);
        }

        public Task<bool> ExpireAsync(string key, TimeSpan expire)
        {
            return _redisClient.ExpireAsync(key, expire);
        }
        public Task<bool> ExpireAsync(string key, int seconds)
        {
            return _redisClient.ExpireAsync(key, seconds);
        }
        public bool ExpireAt(string key, DateTime expire)
        {
            return _redisClient.ExpireAt(key, expire);
        }

        public Task<bool> ExpireAtAsync(string key, DateTime expire)
        {
            return _redisClient.ExpireAtAsync(key, expire);
        }

        public long Increment(string key, long increment = 1)
        {
            return _redisClient.IncrBy(key, increment);
        }

        public Task<long> IncrementAsync(string key, long increment = 1)
        {
            return _redisClient.IncrByAsync(key, increment);
        }

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _redisClient?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
    }
}
