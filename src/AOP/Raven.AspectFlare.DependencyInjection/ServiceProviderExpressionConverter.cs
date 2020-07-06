using Raven.AspectFlare.DynamicProxy;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Raven.AspectFlare.DependencyInjection
{
    public class ServiceProviderExpressionConverter : IExpressionConverter<IServiceProvider, object>
    {
        public bool TryConvert(
            Expression<Func<IServiceProvider, object>> lambdaExpression,
            Type rawType, Type implementType, out Expression<Func<IServiceProvider, object>> convertLambdaExpression)
        {
            if (rawType == null)
            {
                throw new ArgumentNullException(nameof(rawType));
            }

            if (lambdaExpression == null)
            {
                throw new ArgumentNullException(nameof(lambdaExpression));
            }

            if (implementType == null)
            {
                throw new ArgumentNullException(nameof(implementType));
            }

            if (rawType == typeof(void) ||
                rawType == implementType ||
                !rawType.IsAssignableFrom(implementType))
            {
                throw new ArgumentException($"the ReturnType of lambda expression should not be void and is the base class of" +
                    $" the {implementType} class or one of its implemented interfaces");
            }

            var convertVisitor = new ConverterVisitor(rawType, implementType);
            convertLambdaExpression = convertVisitor.Visit(lambdaExpression) as Expression<Func<IServiceProvider, object>>;
            return convertVisitor.IsConvertSuccess;
        }

        private class ConverterVisitor: ExpressionVisitor
        {
            private Type _rawType;
            private Type _implementType;

            public bool IsConvertSuccess { get; private set; }

            public ConverterVisitor(Type rawType, Type implementType)
            {
                _rawType = rawType;
                _implementType = implementType;
            }

            public override Expression Visit(Expression node)
            {
                if (_rawType == null)
                {
                    throw new InvalidOperationException($"Make sure the {_rawType} field value is not null");
                }

                if (_implementType == null)
                {
                    throw new InvalidOperationException($"Make sure the {_implementType} field value is not null");
                }

                IsConvertSuccess = false;
                return base.Visit(node);
            }

            /// <summary>
            /// 替换表达式数中用new的方式来创建代理服务类的节点
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            protected override Expression VisitNew(NewExpression node)
            {
                if (node.Type != _rawType)
                {
                    return base.VisitNew(node);
                }

                var arguments = node.Arguments.Select(a => a.Type).ToArray();
                ConstructorInfo constructor = _implementType
                    .GetConstructors()
                    .FirstOrDefault(ctor =>
                    {
                        var parameters = ctor.GetParameters()
                                                .Select(x => x.ParameterType)
                                                .ToArray();

                        if (parameters.Length != arguments.Length)
                        {
                            return false;
                        }

                        for (int i = 0; i < arguments.Length; i++)
                        {
                            if (parameters[i] != arguments[i])
                                return false;
                        }

                        return true;
                    });

                if (constructor == null)
                {
                    return base.VisitNew(node);
                }

                var newExpression = Expression.New(constructor, node.Arguments);
                IsConvertSuccess = true;
                return newExpression;
            }
        }
    }
}
