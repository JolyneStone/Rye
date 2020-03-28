using KiraNet.AlasFx.Caching;
using KiraNet.AlasFx.DependencyInjection;
using KiraNet.AlasFx.Filter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KiraNet.AlasFx.EntityFrameworkCore
{
    /// <summary>
    /// Queryable扩展方法
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// 异步从指定集合中查询指定数据筛选的分页信息
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TResult">分页数据类型</typeparam>
        /// <param name="source">要查询的数据集</param>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="pageCondition">分页查询条件</param>
        /// <param name="selector">数据筛选表达式</param>
        /// <returns>分页结果信息</returns>
        public static async Task<PageData<TResult>> ToPageAsync<TEntity, TResult>(this IQueryable<TEntity> source,
        Expression<Func<TEntity, bool>> predicate,
        PageCondition pageCondition,
        Expression<Func<TEntity, TResult>> selector)
        {
            Check.NotNull(source, nameof(source));
            Check.NotNull(selector, nameof(selector));

            int total = 0;
            List<TResult> list = null;
            var query = source;
            if (predicate != null)
            {
                query = source.Where(predicate);
            }
            if (pageCondition != null)
            {
                if (pageCondition.SortConditions != null)
                {
                    query = query.OrderByCondition(pageCondition.SortConditions);
                }

                total = await query.CountAsync();
                if (total > 0)
                {
                    query = query.Skip((pageCondition.PageIndex - 1) * pageCondition.PageSize)
                        .Take(pageCondition.PageSize);
                }

                return new PageData<TResult>()
                {
                    Data = list,
                    Total = total
                };
            }
            else
            {
                total = await query.CountAsync();
            }

            if (total > 0)
            {
                list = await query.Select(selector).ToListAsync();
            }

            return new PageData<TResult>
            {
                Data = list ?? new List<TResult>(),
                Total = total
            };
        }

        /// <summary>
        /// 异步查询分页数据结果，如缓存存在，直接返回，否则从数据源查找分页结果，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="predicate"></param>
        /// <param name="pageCondition"></param>
        /// <param name="selector"></param>
        /// <param name="cacheSeconds"></param>
        /// <returns></returns>
        public static Task<PageData<TResult>> ToPageCacheAsync<TEntity, TResult>(this IQueryable<TEntity> source,
            string key,
            Expression<Func<TEntity, bool>> predicate,
            PageCondition pageCondition,
            Expression<Func<TEntity, TResult>> selector,
            int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            IDistributedCache cache = SingleServiceLocator.GetService<IDistributedCache>();
            return cache.GetAsync(key, async () => await source.ToPageAsync(predicate, pageCondition, selector), cacheSeconds);
        }

        /// <summary>
        /// 异步将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">数据源的项数据类型</typeparam>
        /// <typeparam name="TResult">结果集的项数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="key">缓存键</param>
        /// <param name="predicate">数据查询表达式</param>
        /// <param name="selector">数据筛选表达式</param>
        /// <param name="cacheSeconds">缓存时间：秒</param>
        /// <returns></returns>
        public static Task<List<TResult>> ToCacheListAsync<TSource, TResult>(this IQueryable<TSource> source,
            string key,
            Expression<Func<TSource, bool>> predicate,
            Expression<Func<TSource, TResult>> selector,
            int cacheSeconds = 60)
        {
            return source.Where(predicate).ToCacheListAsync(key, selector, cacheSeconds);
        }

        /// <summary>
        /// 异步将结果转换为缓存的数组，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">数据源的项数据类型</typeparam>
        /// <typeparam name="TResult">结果集的项数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="key">缓存键</param>
        /// <param name="predicate">数据查询表达式</param>
        /// <param name="selector">数据筛选表达式</param>
        /// <param name="cacheSeconds">缓存时间：秒</param>
        /// <returns></returns>
        public static Task<TResult[]> ToCacheArrayAsync<TSource, TResult>(this IQueryable<TSource> source,
            string key,
            Expression<Func<TSource, bool>> predicate,
            Expression<Func<TSource, TResult>> selector,
            int cacheSeconds = 60)
        {
            return source.Where(predicate).ToCacheArrayAsync(key, selector, cacheSeconds);
        }

        /// <summary>
        /// 异步将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <typeparam name="TResult">结果集的项数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="key">缓存键</param>
        /// <param name="selector">数据筛选表达式</param>
        /// <param name="cacheSeconds">缓存的秒数</param>
        /// <param name="keyParams">缓存键参数</param>
        /// <returns>查询结果</returns>
        public static Task<List<TResult>> ToCacheListAsync<TSource, TResult>(this IQueryable<TSource> source,
            string key,
            Expression<Func<TSource, TResult>> selector,
            int cacheSeconds = 60)
        {
            IDistributedCache cache = SingleServiceLocator.GetService<IDistributedCache>();
            return cache.GetAsync(key, async () => await source.Select(selector).ToListAsync(), cacheSeconds);
        }

        /// <summary>
        /// 异步将结果转换为缓存的数组，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <typeparam name="TResult">结果集的项数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="key">缓存键</param>
        /// <param name="selector">数据筛选表达式</param>
        /// <param name="cacheSeconds">缓存的秒数</param>
        /// <returns>查询结果</returns>
        public static Task<TResult[]> ToCacheArrayAsync<TSource, TResult>(this IQueryable<TSource> source,
            string key,
            Expression<Func<TSource, TResult>> selector,
            int cacheSeconds = 60)
        {
            IDistributedCache cache = SingleServiceLocator.GetService<IDistributedCache>();
            return cache.GetAsync(key, async () => await source.Select(selector).ToArrayAsync(), cacheSeconds);
        }

        /// <summary>
        /// 异步将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="key">缓存键</param>
        /// <param name="cacheSeconds">缓存的秒数</param>
        /// <param name="keyParams">缓存键参数</param>
        /// <returns>查询结果</returns>
        public static Task<List<TSource>> ToCacheListAsync<TSource>(this IQueryable<TSource> source, string key, int cacheSeconds = 60)
        {
            IDistributedCache cache = SingleServiceLocator.GetService<IDistributedCache>();
            return cache.GetAsync(key, async () => await source.ToListAsync(), cacheSeconds);
        }

        /// <summary>
        /// 异步将结果转换为缓存的数组，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="key">缓存键</param>
        /// <param name="cacheSeconds">缓存的秒数</param>
        /// <param name="keyParams">缓存键参数</param>
        /// <returns>查询结果</returns>
        public static Task<TSource[]> ToCacheArrayAsync<TSource>(this IQueryable<TSource> source, string key, int cacheSeconds = 60)
        {
            IDistributedCache cache = SingleServiceLocator.GetService<IDistributedCache>();
            return cache.GetAsync(key, async () => await source.ToArrayAsync(), cacheSeconds);
        }

        /// <summary>
        /// 异步将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并按指定缓存策略存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="key">缓存键</param>
        /// <returns>查询结果</returns>
        public static Task<List<TSource>> ToCacheListAsync<TSource>(this IQueryable<TSource> source, string key)
        {
            IDistributedCache cache = SingleServiceLocator.GetService<IDistributedCache>();
            return cache.GetAsync(key, async () => await source.ToListAsync());
        }

        /// <summary>
        /// 异步将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并按指定缓存策略存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="key">缓存键</param>
        /// <returns>查询结果</returns>
        public static Task<TSource[]> ToCacheArrayAsync<TSource>(this IQueryable<TSource> source, string key)
        {
            IDistributedCache cache = SingleServiceLocator.GetService<IDistributedCache>();
            return cache.GetAsync(key, async () => await source.ToArrayAsync());
        }
    }
}
