using System.Reflection;
using System.Reflection.Emit;

namespace Raven.AspectFlare.DynamicProxy.Extensions
{
    internal static class ConstructorBuilderExtensions
    {
        public static void SetMethodParameters(this ConstructorBuilder ctorBuilder, ParameterInfo[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                var baseParameter = parameters[i];
                var parameterBuilder = ctorBuilder.DefineParameter(
                            i + 1,
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
