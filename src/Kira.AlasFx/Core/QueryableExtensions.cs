using Kira.AlasFx.Filter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Kira.AlasFx.Core
{
    /// <summary>
    /// Queryable扩展方法
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// 动态排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderCondition"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderByCondition<T>(this IQueryable<T> query, IEnumerable<SortCondition> orderCondition)
        {
            var type = typeof(T);
            if (orderCondition != null)
            {
                foreach (var condition in orderCondition)
                {
                    query = OrderByCondition<T>(query, condition.Field, condition.SortDirection);
                }
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
                var propertyInfo = type.GetProperty(field, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                var parameter = Expression.Parameter(type);
                Expression propertySelector = Expression.Property(parameter, propertyInfo);

                var orderby = Expression.Lambda<Func<T, object>>(Expression.Convert(propertySelector, typeof(object)), parameter);
                if (ListSortDirection.Ascending == sortDirection)
                    query = query.OrderBy(orderby);
                else if (ListSortDirection.Descending == sortDirection)
                    query = query.OrderByDescending(orderby);
            }
            return query;
        }
    }
}
