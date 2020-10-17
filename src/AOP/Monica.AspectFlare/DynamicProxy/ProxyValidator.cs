using Monica.AspectFlare.DynamicProxy;
using System;
using System.Linq;
using System.Reflection;

namespace Monica.AspectFlare
{
    public class ProxyValidator : IProxyValidator
    {
        private static readonly Type NonIntercept = typeof(NonInterceptAttribute);
        private static readonly Type CallingIntercept = typeof(CallingInterceptAttribute);
        private static readonly Type CalledIntercept = typeof(CalledInterceptAttribute);
        private static readonly Type ExceptionIntercept = typeof(ExceptionInterceptAttribute);
        private static readonly Type Intercept = typeof(InterceptAttribute);

        public bool Validate(Type classType)
        {
            if (classType == null)
            {
                throw new ArgumentNullException(nameof(classType));
            }

            if (!classType.IsVisible ||
                classType.IsValueType ||
                classType.IsSealed)
            {
                return false;
            }

            bool hasClassIntercept = classType.HasInterceptAttribute();
            foreach (var proxyCtor in classType.GetConstructors(
                                        BindingFlags.CreateInstance |
                                        BindingFlags.Instance |
                                        BindingFlags.Public |
                                        BindingFlags.NonPublic
                                    ).Where(
                                    x =>
                                        x.IsPublic ||
                                        x.IsFamily ||
                                        !(x.IsAssembly || x.IsFamilyAndAssembly || x.IsFamilyOrAssembly)
                            ))
            {
                if (proxyCtor.IsDefined(typeof(NonInterceptAttribute)))
                {
                    continue;
                }

                if (GlobalInterceptorCollection.GlobalInterceptors.Count > 0)
                {
                    return true;
                }

                if (!proxyCtor.HasDefineInterceptAttribute() && !hasClassIntercept)
                {
                    continue;
                }

                return true;
            }

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

                if (GlobalInterceptorCollection.GlobalInterceptors.Count > 0)
                {
                    return true;
                }

                if (!proxyMethod.HasDefineInterceptAttribute() && !hasClassIntercept)
                {
                    continue;
                }

                if (proxyMethod.ReturnType.IsByRef)
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        public bool Validate(Type interfaceType, Type classType)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            if (classType == null)
            {
                throw new ArgumentNullException(nameof(classType));
            }

            bool hasClassIntercept = classType.HasInterceptAttribute();
            bool hasInterfaceIntercept = interfaceType.HasInterceptAttribute();
            var interfaceMethods = interfaceType.GetMethods(
                 BindingFlags.Public |
                 BindingFlags.Instance |
                 BindingFlags.DeclaredOnly
             );

            foreach(var interfaceMethod in interfaceMethods)
            {
                if (interfaceMethod.IsDefined(typeof(NonInterceptAttribute)))
                {
                    continue;
                }

                if (GlobalInterceptorCollection.GlobalInterceptors.Count > 0)
                {
                    return true;
                }

                if (!interfaceMethod.HasDefineInterceptAttribute() && !hasInterfaceIntercept)
                {
                    continue;
                }

                return true;
            }

            var impNames = interfaceType.GetMethods(
                 BindingFlags.Public |
                 BindingFlags.Instance |
                 BindingFlags.DeclaredOnly
             )
             .Select(x => x.Name);

            var impImpNames = impNames.Select(x => x); // 隐式接口实现方法名称
            var expImpNames = impNames.Select(x => interfaceType.FullName + "." + x); // 显示接口实现方法名称
            foreach (var proxyMethod in classType.GetMethods(
                            BindingFlags.Instance |
                            BindingFlags.Public |
                            BindingFlags.NonPublic
                        ).Where(
                         x => x.IsVirtual
                ))
            {
                if (proxyMethod.IsDefined(typeof(NonInterceptAttribute)))
                {
                    continue;
                }

                if (GlobalInterceptorCollection.GlobalInterceptors.Count > 0)
                {
                    return true;
                }

                if (!proxyMethod.HasDefineInterceptAttribute() && !hasClassIntercept && !hasInterfaceIntercept)
                {
                    continue;
                }

                if (!impImpNames.Contains(proxyMethod.Name) && !expImpNames.Contains(proxyMethod.Name))
                {
                    continue;
                }

                return true;
            }

            return false;
        }
    }
}
