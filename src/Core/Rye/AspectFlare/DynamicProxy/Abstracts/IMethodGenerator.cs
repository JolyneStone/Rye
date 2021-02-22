using System.Reflection;
using System.Reflection.Emit;

namespace Rye.AspectFlare.DynamicProxy
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
