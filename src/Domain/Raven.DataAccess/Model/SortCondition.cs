using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Raven.DataAccess
{
    /// <summary>
    /// 排序条件
    /// </summary>
    public class SortCondition
    {
        public SortCondition() : this(null)
        {
        }

        public SortCondition(string sortField) : this(sortField, ListSortDirection.Ascending)
        {
        }

        public SortCondition(string sortField, ListSortDirection listSortDirection)
        {
            Field = sortField;
            SortDirection = listSortDirection;
        }

        /// <summary>
        /// 排序字段名称
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 排序方向
        /// </summary>
        public ListSortDirection SortDirection { get; set; }
    }

    public class SortCondition<T> : SortCondition
    {
        public SortCondition(Expression<Func<T, object>> keySelector) : this(keySelector, ListSortDirection.Ascending)
        {
        }

        public SortCondition(Expression<Func<T, object>> keySelector, ListSortDirection listSortDirection) : base(GetPropertyName(keySelector), listSortDirection)
        {
        }

        /// <summary>
        /// 从泛型委托获取属性名
        /// </summary>
        private static string GetPropertyName(Expression<Func<T, object>> keySelector)
        {
            string param = keySelector.Parameters.First().Name;
            string operand = ((dynamic)keySelector.Body).Operand.ToString();
            operand = operand.Substring(param.Length + 1, operand.Length - param.Length - 1);
            return operand;
        }
    }
}
