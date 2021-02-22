using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Rye.Core
{
    internal sealed class CloneObject<TSource, TTarget> where TTarget : TSource
    {
        private static readonly Func<TSource, TTarget> _funcCache = GetFunc();
        private static readonly Action<TSource, TTarget> _actionCache = GetAction();

        private static Func<TSource, TTarget> GetFunc()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TSource), "p");
            List<MemberBinding> memberBindingList = new List<MemberBinding>();

            var targetType = typeof(TTarget);
            foreach (var item in typeof(TSource).GetProperties())
            {
                if (!item.CanWrite || !item.CanRead)
                    continue;
                var setProperty = targetType.GetProperty(item.Name);
                MemberExpression property = Expression.Property(parameterExpression, item);
                MemberBinding memberBinding = Expression.Bind(setProperty, property);
                memberBindingList.Add(memberBinding);
            }

            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TTarget)), memberBindingList.ToArray());
            Expression<Func<TSource, TTarget>> lambda = Expression.Lambda<Func<TSource, TTarget>>(memberInitExpression, new ParameterExpression[] { parameterExpression });

            return lambda.Compile();
        }

        private static Action<TSource, TTarget> GetAction()
        {
            ParameterExpression parameterExpressionSource = Expression.Parameter(typeof(TSource), "p");
            ParameterExpression parameterExpressionTarget = Expression.Parameter(typeof(TTarget), "q");
            List<Expression> memberAssingList = new List<Expression>();

            var targetType = typeof(TTarget);
            foreach (var item in typeof(TSource).GetProperties())
            {
                if (!item.CanWrite || !item.CanRead)
                    continue;

                var setProperty = targetType.GetProperty(item.Name);
                var getMethod = item.GetGetMethod();
                var setMethod = setProperty.GetSetMethod();

                if (getMethod != null && setMethod != null)
                {
                    var assign = Expression.Call(parameterExpressionTarget, setMethod, Expression.Call(parameterExpressionSource, getMethod));
                    memberAssingList.Add(assign);
                }
            }

            Expression<Action<TSource, TTarget>> lambda = Expression.Lambda<Action<TSource, TTarget>>(Expression.Block(memberAssingList), new ParameterExpression[] { parameterExpressionSource, parameterExpressionTarget });

            return lambda.Compile();
        }

        public static TTarget Clone(TSource source)
        {
            return _funcCache(source);
        }

        public static void Clone(TSource source, TTarget target)
        {
            _actionCache(source, target);
        }
    }
}
