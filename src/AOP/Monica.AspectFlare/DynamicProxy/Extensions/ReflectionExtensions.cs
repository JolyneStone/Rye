using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Monica.AspectFlare.DynamicProxy
{
    public static class ReflectionExtensions
    {
        private static readonly Type NonIntercept = typeof(NonInterceptAttribute);
        private static readonly Type CallingIntercept = typeof(CallingInterceptAttribute);
        private static readonly Type CalledIntercept = typeof(CalledInterceptAttribute);
        private static readonly Type ExceptionIntercept = typeof(ExceptionInterceptAttribute);

        public static bool TryGetDefaultValue(this ParameterInfo parameter, out object defaultValue)
        {
            bool hasDefaultValue;
            var tryToGetDefaultValue = true;
            defaultValue = null;

            try
            {
                hasDefaultValue = parameter.HasDefaultValue;
            }
            catch (FormatException) when (parameter.ParameterType == typeof(DateTime)) // 如果该参数类型为DateTime，则会抛出此异常 因为DateTime的任何值都不是编译时常量
                                                                                       // 详见 https://github.com/dotnet/corefx/issues/12338
            {
                hasDefaultValue = true;
                tryToGetDefaultValue = false;
            }

            if (hasDefaultValue)
            {
                if (tryToGetDefaultValue)
                {
                    defaultValue = parameter.DefaultValue;
                }

                if (defaultValue == null && parameter.ParameterType.GetTypeInfo().IsValueType)
                {
                    defaultValue = Activator.CreateInstance(parameter.ParameterType);
                }
            }

            return hasDefaultValue;
        }

        public static bool HasInterceptAttribute(this Type type)
        {
            return (!type.IsDefined(NonIntercept, true)) && (
                          type.IsDefined(CallingIntercept, true) ||
                           type.IsDefined(CalledIntercept, true) ||
                            type.IsDefined(ExceptionIntercept, true)
                        );
        }

        public static bool HasInterceptAttribute(this MethodBase method)
        {
            return (!method.IsDefined(NonIntercept, true)) && (
                          method.IsDefined(CallingIntercept, true) ||
                           method.IsDefined(CalledIntercept, true) ||
                            method.IsDefined(ExceptionIntercept, true)
                        );
        }

        public static bool HasDefineInterceptAttribute(this MethodBase method)
        {
            return method.IsDefined(CallingIntercept, true) ||
                    method.IsDefined(CalledIntercept, true) ||
                     method.IsDefined(ExceptionIntercept, true);
        }

        public static bool CanAsClassInterceptMethod(this MethodBase method, bool hasClassIntercept)
        {
            if (!method.IsVirtual)
            {
                return false;
            }

            return HasInterceptAttribute(method) ? true : hasClassIntercept;
        }

        public static bool CanAsInterfaceInterceptMethod(this MethodBase method, bool hasInterfaceIntercept)
        {
            return HasInterceptAttribute(method) ? true : hasInterfaceIntercept;
        }

        public static IEnumerable<MethodInfo> GetAsClassInterceptMethods(this Type type)
        {
            var hasClassIntercept = HasInterceptAttribute(type);
            return type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => x.CanAsClassInterceptMethod(hasClassIntercept));
        }

        public static IEnumerable<MethodInfo> GetAsInterfaceInterceptMethods(this Type type)
        {
            var hasInterfaceIntercept = HasInterceptAttribute(type);
            return type.GetMethods()
                .Where(x => x.CanAsInterfaceInterceptMethod(hasInterfaceIntercept));
        }

        public static IEnumerable<ConstructorInfo> GetAsConstructorInterceptMethods(this Type type)
        {
            var hasInterfaceIntercept = HasInterceptAttribute(type);
            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => x.CanAsInterfaceInterceptMethod(hasInterfaceIntercept));
        }

        private static void AddRangeWrapper(IEnumerable<MethodBase> methods, IDictionary<int, InterceptorWrapper> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = new Dictionary<int, InterceptorWrapper>();
            }

            foreach (var method in methods)
            {
                var methodCallingInterceptors = method.GetCustomAttributes<CallingInterceptAttribute>(true).OfType<ICallingInterceptor>().ToList();
                var methodCalledInterceptors = method.GetCustomAttributes<CalledInterceptAttribute>(true).OfType<ICalledInterceptor>().ToList();
                var methodExceptionInterceptor = method.GetCustomAttribute<ExceptionInterceptAttribute>(true) as IExceptionInterceptor;

                dictionary.Add(method.MetadataToken, new InterceptorWrapper
                {
                    CallingInterceptors = methodCallingInterceptors,
                    CalledInterceptors = methodCalledInterceptors,
                    ExceptionInterceptor = methodExceptionInterceptor
                });
            }
        }

        //public static IDictionary<int, InterceptorWrapper> GetInterceptorWrapperDictionary(this Type type, Type interfaceType = null)
        //{
        //    Dictionary<int, InterceptorWrapper> wrappers = new Dictionary<int, InterceptorWrapper>();
        //    IEnumerable<MethodInfo> methods;
        //    if (interfaceType != null && interfaceType.IsInterface)
        //    {
        //        methods = GetAsInterfaceInterceptMethods(interfaceType);
        //    }
        //    else
        //    {
        //        methods = GetAsClassInterceptMethods(type);
        //    }

        //    AddRangeWrapper(methods, wrappers);

        //    var constructors = GetAsConstructorInterceptMethods(type);
        //    AddRangeWrapper(constructors, wrappers);

        //    return wrappers;
        //}
    }
}
