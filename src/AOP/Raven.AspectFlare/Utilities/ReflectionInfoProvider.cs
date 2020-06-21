using System;
using System.Reflection;
using Raven.AspectFlare.DynamicProxy;

namespace Raven.AspectFlare.Utilities
{
    internal static class ReflectionInfoProvider
    {
        private static ConstructorInfo _objectConstructor;
        private static ConstructorInfo _interceptorWrapperCollectionByClass;
        private static ConstructorInfo _interceptorWrapperCollectionByInterface;
        private static MethodInfo _getTypeFromHandle;
        private static MethodInfo _getWrapper;
    
        public static MethodInfo GetTypeFromHandle
        {
            get
            {
                if (_getTypeFromHandle == null)
                {
                    _getTypeFromHandle = typeof(Type).GetMethod(
                                            "GetTypeFromHandle",
                                            BindingFlags.Public |
                                            BindingFlags.Instance |
                                            BindingFlags.Static
                                        );
                }

                return _getTypeFromHandle;
            }
        }

        public static ConstructorInfo InterceptorWrapperCollectionByClass
        {
            get
            {
                if (_interceptorWrapperCollectionByClass == null)
                {
                    var type = typeof(Type);
                    _interceptorWrapperCollectionByClass = typeof(InterceptorWrapperCollection)
                                .GetConstructor(
                                    BindingFlags.Public |
                                    BindingFlags.Instance,
                                    null,
                                    new Type[] { type, type },
                                    null
                                );
                }

                return _interceptorWrapperCollectionByClass;
            }
        }

        public static ConstructorInfo InterceptorWrapperCollectionByInterface
        {
            get
            {
                if (_interceptorWrapperCollectionByInterface == null)
                {
                    var type = typeof(Type);
                    _interceptorWrapperCollectionByInterface = typeof(InterceptorWrapperCollection)
                                .GetConstructor(
                                    BindingFlags.Public |
                                    BindingFlags.Instance,
                                    null,
                                    new Type[] { type, type, type },
                                    null
                                );
                }

                return _interceptorWrapperCollectionByInterface;
            }
        }

        public static MethodInfo GetWrapper
        {
            get
            {
                if (_getWrapper == null)
                {
                    _getWrapper = typeof(InterceptorWrapperCollection)
                                .GetMethod(
                                    "GetWrapper",
                                    BindingFlags.Instance |
                                    BindingFlags.Public,
                                    null,
                                    new Type[] { typeof(int) },
                                    null
                                );
                }

                return _getWrapper;
            }
        }


        public static ConstructorInfo ObjectConstructor
        {
            get
            {
                if (_objectConstructor == null)
                {
                    _objectConstructor = typeof(object).GetConstructor(
                        BindingFlags.Public | BindingFlags.Instance,
                        null,
                        Type.EmptyTypes,
                        null
                    );
                }

                return _objectConstructor;
            }
        }
    }
}
