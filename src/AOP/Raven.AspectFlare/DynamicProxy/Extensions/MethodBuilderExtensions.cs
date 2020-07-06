using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Raven.AspectFlare.DynamicProxy.Extensions
{
    internal static class MethodBuilderExtensions
    {
        public static void SetMethodParameters(this MethodBuilder methodBuilder, MethodBase method, ParameterInfo[] parameters)
        {
            if (method.IsGenericMethodDefinition)
            {
                var genericArguments = method.GetGenericArguments();
                GenericTypeParameterBuilder[] typeArguments = methodBuilder.DefineGenericParameters(
                                                                    genericArguments.Select(x => x.Name).ToArray()
                                                            );
                for (var i = 0; i < genericArguments.Length; i++)
                {
                    var typeArgument = typeArguments[i];
                    var genericArgument = genericArguments[i];
                    typeArgument.SetGenericParameterAttributes(genericArguments[i].GenericParameterAttributes);
                    typeArgument.SetBaseTypeConstraint(genericArgument.BaseType);
                    typeArgument.SetInterfaceConstraints(genericArgument.GetInterfaces());
                }
            }

            for (var i = 0; i < parameters.Length; i++)
            {
                var baseParameter = parameters[i];
                var parameterBuilder = methodBuilder.DefineParameter(
                            i + 1,   // TODO
                            baseParameter.Attributes,
                            baseParameter.Name
                        );

                if (baseParameter.HasDefaultValue && baseParameter.TryGetDefaultValue(out object defaultValue))
                {
                    parameterBuilder.SetConstant(defaultValue);
                }
            }
        }
    }
}
