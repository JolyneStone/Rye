using System;
using System.Linq;
using System.Reflection.Emit;

namespace Raven.AspectFlare.DynamicProxy
{
    internal class DefineTypeOperator : IGenerateTypeOperator
    {
        public void Generate(GeneratorTypeContext context)
        {
            var classType = context.ClassType;
            if (context.InterfaceType == null)
            {  
                var typeBuilder = context.ModuleBuilder.DefineType(
                        $"{classType.Name}_AspectFlare",
                        classType.Attributes
                    );

                typeBuilder.SetParent(classType);
                context.TypeBuilder = typeBuilder;
            }
            else
            {
                context.TypeBuilder = context.ModuleBuilder.DefineType(
                         $"<AspectFlare>{classType.Name}",
                         classType.Attributes,
                         typeof(object),
                         new Type[] { context.InterfaceType }
                     );
            }

            if(classType.IsGenericTypeDefinition)
            {
                GenerateGeneric(classType, context.TypeBuilder);
            }
        }

        private void GenerateGeneric(Type type, TypeBuilder typeBuilder)
        {
            var genericArguments = type.GetGenericArguments();
            GenericTypeParameterBuilder[] typeArguments = typeBuilder.DefineGenericParameters(
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
    }
}
