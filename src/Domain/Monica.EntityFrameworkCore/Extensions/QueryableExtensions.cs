using Monica.Cache;
using Monica.DependencyInjection;
using Monica.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;

namespace Monica.EntityFrameworkCore
{
    /// <summary>
    /// Queryable扩展方法
    /// </summary>
    public static class QueryableExtensions
    {
        private static Expression<Func<T, object>> GenerateSelector<T>(Type type, string field)
        {
            var propertyInfo = type.GetProperty(field, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            var parameter = Expression.Parameter(type);
            Expression propertySelector = Expression.Property(parameter, propertyInfo);
            return Expression.Lambda<Func<T, object>>(Expression.Convert(propertySelector, typeof(object)), parameter);
        }

        /// <summary>
        /// 动态排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="sortConditions"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderByCondition<T>(this IQueryable<T> query, IEnumerable<SortCondition> sortConditions)
        {
            var type = typeof(T);
            if (sortConditions != null)
            {
                var i = 0;
                IOrderedQueryable<T> orderQuery = null;
                foreach (var condition in sortConditions)
                {
                    var orderby = GenerateSelector<T>(type, condition.Field);
                    if (ListSortDirection.Ascending == condition.SortDirection)
                        orderQuery = i == 0 ? query.OrderBy(orderby) : Queryable.ThenBy(orderQuery, orderby);
                    else if (ListSortDirection.Descending == condition.SortDirection)
                        orderQuery = i == 0 ? orderQuery.OrderByDescending(orderby) : Queryable.ThenByDescending(orderQuery, orderby);
                    i++;
                }
                if (orderQuery != null)
                    return orderQuery;
            }

            return query;
        }
        /// <summary>
        /// 动态排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="sortCondition"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderByCondition<T>(this IQueryable<T> query, SortCondition sortCondition)
        {
            return OrderByCondition<T>(query, sortCondition.Field, sortCondition.SortDirection);
        }

        /// <summary>
        /// 动态排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="field"></param>
        /// <param name="sortDirection"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderByCondition<T>(this IQueryable<T> query, string field, ListSortDirection sortDirection)
        {
            var type = typeof(T);
            if (!string.IsNullOrEmpty(field))
            {
                var orderby = GenerateSelector<T>(type, field);
                if (ListSortDirection.Ascending == sortDirection)
                    query = query.OrderBy(orderby);
                else if (ListSortDirection.Descending == sortDirection)
                    query = query.OrderByDescending(orderby);
            }
            return query;
        }

        /// <summary>
        /// 查询分页数据结果，如缓存存在，直接返回，否则从数据源查找分页结果，并存入缓存中再返回
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
        public static PageData<TResult> ToPageCache<TEntity, TResult>(this IQueryable<TEntity> source,
            string key,
            Expression<Func<TEntity, bool>> predicate,
            PageCondition pageCondition,
            Expression<Func<TEntity, TResult>> selector,
            int cacheSeconds = 60)
        {
            Check.NotNullOrEmpty(key, nameof(key));
            IDistributedCache cache = SingleServiceLocator.GetService<IDistributedCache>();
            return cache.Get(key, () => source.ToPage(predicate, pageCondition, selector), cacheSeconds);
        }

        /// <summary>
        /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">数据源的项数据类型</typeparam>
        /// <typeparam name="TResult">结果集的项数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="key">缓存键</param>
        /// <param name="predicate">数据查询表达式</param>
        /// <param name="selector">数据筛选表达式</param>
        /// <param name="cacheSeconds">缓存时间：秒</param>
        /// <returns></returns>
        public static List<TResult> ToCacheList<TSource, TResult>(this IQueryable<TSource> source,
            string key,
            Expression<Func<TSource, bool>> predicate,
            Expression<Func<TSource, TResult>> selector,
            int cacheSeconds = 60)
        {
            return source.Where(predicate).ToCacheList(key, selector, cacheSeconds);
        }

        /// <summary>
        /// 将结果转换为缓存的数组，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">数据源的项数据类型</typeparam>
        /// <typeparam name="TResult">结果集的项数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="key">缓存键</param>
        /// <param name="predicate">数据查询表达式</param>
        /// <param name="selector">数据筛选表达式</param>
        /// <param name="cacheSeconds">缓存时间：秒</param>
        /// <returns></returns>
        public static TResult[] ToCacheArray<TSource, TResult>(this IQueryable<TSource> source,
            string key,
            Expression<Func<TSource, bool>> predicate,
            Expression<Func<TSource, TResult>> selector,
            int cacheSeconds = 60)
        {
            return source.Where(predicate).ToCacheArray(key, selector, cacheSeconds);
        }

        /// <summary>
        /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <typeparam name="TResult">结果集的项数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="key">缓存键</param>
        /// <param name="selector">数据筛选表达式</param>
        /// <param name="cacheSeconds">缓存的秒数</param>
        /// <param name="keyParams">缓存键参数</param>
        /// <returns>查询结果</returns>
        public static List<TResult> ToCacheList<TSource, TResult>(this IQueryable<TSource> source,
            string key,
            Expression<Func<TSource, TResult>> selector,
            int cacheSeconds = 60)
        {
            IDistributedCache cache = SingleServiceLocator.GetService<IDistributedCache>();
            return cache.Get(key, () => source.Select(selector).ToList(), cacheSeconds);
        }

        /// <summary>
        /// 将结果转换为缓存的数组，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <typeparam name="TResult">结果集的项数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="key">缓存键</param>
        /// <param name="selector">数据筛选表达式</param>
        /// <param name="cacheSeconds">缓存的秒数</param>
        /// <returns>查询结果</returns>
        public static TResult[] ToCacheArray<TSource, TResult>(this IQueryable<TSource> source,
            string key,
            Expression<Func<TSource, TResult>> selector,
            int cacheSeconds = 60)
        {
            IDistributedCache cache = SingleServiceLocator.GetService<IDistributedCache>();
            return cache.Get(key, () => source.Select(selector).ToArray(), cacheSeconds);
        }

        /// <summary>
        /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="key">缓存键</param>
        /// <param name="cacheSeconds">缓存的秒数</param>
        /// <param name="keyParams">缓存键参数</param>
        /// <returns>查询结果</returns>
        public static List<TSource> ToCacheList<TSource>(this IQueryable<TSource> source, string key, int cacheSeconds = 60)
        {
            IDistributedCache cache = SingleServiceLocator.GetService<IDistributedCache>();
            return cache.Get(key, source.ToList, cacheSeconds);
        }

        /// <summary>
        /// 将结果转换为缓存的数组，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="key">缓存键</param>
        /// <param name="cacheSeconds">缓存的秒数</param>
        /// <param name="keyParams">缓存键参数</param>
        /// <returns>查询结果</returns>
        public static TSource[] ToCacheArray<TSource>(this IQueryable<TSource> source, string key, int cacheSeconds = 60)
        {
            IDistributedCache cache = SingleServiceLocator.GetService<IDistributedCache>();
            return cache.Get(key, source.ToArray, cacheSeconds);
        }

        /// <summary>
        /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并按指定缓存策略存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="key">缓存键</param>
        /// <returns>查询结果</returns>
        public static List<TSource> ToCacheList<TSource>(this IQueryable<TSource> source, string key)
        {
            IDistributedCache cache = SingleServiceLocator.GetService<IDistributedCache>();
            return cache.Get(key, source.ToList);
        }

        /// <summary>
        /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并按指定缓存策略存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="key">缓存键</param>
        /// <returns>查询结果</returns>
        public static TSource[] ToCacheArray<TSource>(this IQueryable<TSource> source, string key)
        {
            IDistributedCache cache = SingleServiceLocator.GetService<IDistributedCache>();
            return cache.Get(key, source.ToArray);
        }

        /// <summary>
        /// 从指定集合中查询指定数据筛选的分页信息
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TResult">分页数据类型</typeparam>
        /// <param name="source">要查询的数据集</param>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="pageCondition">分页查询条件</param>
        /// <param name="selector">数据筛选表达式</param>
        /// <returns>分页结果信息</returns>
        public static PageData<TResult> ToPage<TEntity, TResult>(this IQueryable<TEntity> source,
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

                total = query.Count();
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
                total = query.Count();
            }

            if (total > 0)
            {
                list = query.Select(selector).ToList();
            }

            return new PageData<TResult>
            {
                Data = list ?? new List<TResult>(),
                Total = total
            };
        }

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
