using System.Reflection;
using System.Reflection.Emit;

namespace Raven.AspectFlare.DynamicProxy
{
    public interface IMethodGenerator
    {
        void GenerateMethod(
            GeneratorTypeContext context,
            ILGenerator methodGenerator,
            MethodBuilder methodBuilder,
            MethodBase method,
            ParameterInfo[] parameters);
    }
}
