using System.Reflection;

namespace Rye.AspectFlare.DynamicProxy
{
    internal class DefineFieldsOperator : IGenerateTypeOperator
    {
        public void Generate(GeneratorTypeContext context)
        {
            context.Wrappers = context.TypeBuilder.DefineField(
                "<_wrappers>",
                typeof(InterceptorWrapperCollection),
                FieldAttributes.Private
            );

            if(context.InterfaceType != null)
            {
                context.Interface = context.TypeBuilder.DefineField(
                    $"<{context.ClassType.Name}>_i",
                    context.InterfaceType,
                    FieldAttributes.Private
                );
            }
        }
    }
}

