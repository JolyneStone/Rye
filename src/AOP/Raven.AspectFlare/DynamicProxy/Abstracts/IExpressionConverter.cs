using System;
using System.Linq.Expressions;

namespace Raven.AspectFlare.DynamicProxy
{
    public interface IExpressionConverter<TServer, TProxy>
            where TServer : class
            where TProxy : class
    {
        bool TryConvert(
            Expression<Func<TServer, TProxy>> lambdaExpression,
            Type rawType,
            Type implementType,
            out Expression<Func<TServer, TProxy>> convertLambdaExpression
        );
    }
}
