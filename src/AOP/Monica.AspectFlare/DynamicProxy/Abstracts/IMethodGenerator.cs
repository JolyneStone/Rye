using System.Reflection;
using System.Reflection.Emit;

namespace Monica.AspectFlare.DynamicProxy
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
