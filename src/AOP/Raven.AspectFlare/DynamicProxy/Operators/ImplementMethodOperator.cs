using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Raven.AspectFlare.DynamicProxy.Extensions;

namespace Raven.AspectFlare.DynamicProxy
{
    internal class ImplementMethodOperator : GenerateMethodOperator
    {
        public override void Generate(GeneratorTypeContext context)
        {
            if (context.InterfaceType != null)
            {
                GenerateInterface(context);
            }
            else
            {
                GenerateClass(context);
            }
        }

        private void GenerateInterface(GeneratorTypeContext context)
        {
            var interfaceType = context.InterfaceType;
            var impNames = interfaceType.GetMethods(
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.DeclaredOnly
            );

            var impImpNames = impNames.Select(x => x.Name); // 隐式接口实现方法名称
            var expImpNames = impNames.Select(x => interfaceType.FullName + "." + x.Name); // 显示接口实现方法名称


            var classType = context.ClassType;
            var typeBuilder = context.TypeBuilder;
            bool hasClassIntercept = classType.HasInterceptAttribute();
            bool hasInterfaceIntercept = interfaceType.HasInterceptAttribute();
            var interfaceMethods = interfaceType.GetMethods(
                 BindingFlags.Public |
                 BindingFlags.Instance |
                 BindingFlags.DeclaredOnly
             );

            foreach (var proxyMethod in classType.GetMethods(
                            BindingFlags.Instance |
                            BindingFlags.Public |
                            BindingFlags.NonPublic
                        ).Where(
                         x => x.IsVirtual
                ))
            {
                //if (proxyMethod.IsDefined(typeof(NonInterceptAttribute)))
                //{
                //    continue;
                //}

                if (!impImpNames.Contains(proxyMethod.Name) && !expImpNames.Contains(proxyMethod.Name))
                {
                    continue;
                }

                //if (!proxyMethod.HasDefineInterceptAttribute() && !hasClassIntercept && !hasInterfaceIntercept)
                //{
                //    continue;
                //}


                context.MethodHandles.Add(proxyMethod.MethodHandle);

                var baseParameterInfos = proxyMethod.GetParameters();

                // 定义方法
                MethodBuilder methodBuilder = typeBuilder.DefineMethod(
                        interfaceType.FullName + "." + proxyMethod.Name,
                        MethodAttributes.Private | MethodAttributes.HideBySig |
                        MethodAttributes.NewSlot | MethodAttributes.Virtual |
                        MethodAttributes.Final,
                        CallingConventions.HasThis | CallingConventions.Standard,
                        proxyMethod.ReturnType,
                        baseParameterInfos.Select(x => x.ParameterType).ToArray()
                    );

                methodBuilder.SetReturnType(proxyMethod.ReturnType);
                methodBuilder.SetMethodParameters(proxyMethod, baseParameterInfos);

                ILGenerator methodGenerator = methodBuilder.GetILGenerator();

                var dotIndex = proxyMethod.Name.LastIndexOf('.');
                var interfaceMethod = impNames.First(x =>
                    x.Name == proxyMethod.Name.Substring(dotIndex == -1 ? 0 : dotIndex));

                GenerateMethod(methodBuilder, proxyMethod, interfaceMethod, methodGenerator, context, baseParameterInfos);
                typeBuilder.DefineMethodOverride(methodBuilder, interfaceMethod);
            }
        }

        private void GenerateClass(GeneratorTypeContext context)
        {
            var classType = context.ClassType;
            var typeBuilder = context.TypeBuilder;
            bool hasClassIntercept = classType.HasInterceptAttribute();

            foreach (var proxyMethod in classType.GetMethods(
                                        BindingFlags.Instance |
                                        BindingFlags.Public |
                                        BindingFlags.NonPublic
                                    ).Where(
                                     x =>
                                       x.IsVirtual &&
                                      (x.IsPublic || x.IsFamily)
                            ))
            {
                if (proxyMethod.IsDefined(typeof(NonInterceptAttribute)))
                {
                    continue;
                }

                if (!proxyMethod.HasDefineInterceptAttribute() && !hasClassIntercept)
                {
                    continue;
                }

                if (proxyMethod.ReturnType.IsByRef)
                {
                    continue;
                }

                context.MethodHandles.Add(proxyMethod.MethodHandle);

                var baseParameterInfos = proxyMethod.GetParameters();

                // 定义方法
                MethodBuilder methodBuilder = typeBuilder.DefineMethod(
                        proxyMethod.Name,
                        proxyMethod.Attributes ^ MethodAttributes.NewSlot,
                        CallingConventions.HasThis | CallingConventions.Standard,
                        proxyMethod.ReturnType,
                        baseParameterInfos.Select(x => x.ParameterType).ToArray()
                    );

                methodBuilder.SetReturnType(proxyMethod.ReturnType);
                methodBuilder.SetMethodParameters(proxyMethod, baseParameterInfos);

                ILGenerator methodGenerator = methodBuilder.GetILGenerator();
                GenerateMethod(methodBuilder, proxyMethod, null, methodGenerator, context, baseParameterInfos);
                typeBuilder.DefineMethodOverride(methodBuilder, proxyMethod);
            }
        }
    }
}
