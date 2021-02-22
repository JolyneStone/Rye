using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Rye.AspectFlare.Utilities;

namespace Rye.AspectFlare.DynamicProxy
{
    public class InterceptorWrapperCollection : IEnumerable<KeyValuePair<int, InterceptorWrapper>>
    {
        private readonly Dictionary<int, InterceptorWrapper> _wrappers;

        public InterceptorWrapperCollection(Type interfaceType, Type classType, Type proxyType)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            if (classType == null)
            {
                throw new ArgumentNullException(nameof(classType));
            }

            if (proxyType == null)
            {
                throw new ArgumentNullException(nameof(proxyType));
            }

            _wrappers = new Dictionary<int, InterceptorWrapper>();
            SetClassInterceptors(_wrappers, classType);
            if (!interfaceType.IsDefined(typeof(NonInterceptAttribute)))
            {
                var (callingInterceptors, calledInterceptors) = GetMemberInterceptorsWithoutException(interfaceType);
                if (_wrappers.ContainsKey(0))
                {
                    _wrappers[0].CallingInterceptors.AddRange(callingInterceptors);
                    _wrappers[0].CalledInterceptors.AddRange(calledInterceptors);
                    if (_wrappers[0].ExceptionInterceptor == null)
                    {
                        _wrappers[0].ExceptionInterceptor = interfaceType.GetCustomAttribute<ExceptionInterceptAttribute>();
                    }
                }
                else
                {
                    _wrappers.Add(0, new InterceptorWrapper
                    {
                        CallingInterceptors = new List<ICallingInterceptor>(callingInterceptors),
                        CalledInterceptors = new List<ICalledInterceptor>(calledInterceptors),
                        ExceptionInterceptor = interfaceType.GetCustomAttribute<ExceptionInterceptAttribute>()
                    });
                }
            }

            var nonInterceptAttribute = typeof(NonInterceptAttribute);
            var interfaceMethods = interfaceType.GetMethods(
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.DeclaredOnly
            );

            foreach (var methodHandle in HandleCollection.GetHandles(proxyType.MetadataToken))
            {
                var method = MethodBase.GetMethodFromHandle(methodHandle);
                if (!method.IsDefined(nonInterceptAttribute))
                {
                    var (callingMethodInterceptors, calledMethodInterceptors, exceptionMethodInterceptor) = GetMemberInterceptors(method);
                    var wrapper = new InterceptorWrapper
                    {
                        CallingInterceptors = new List<ICallingInterceptor>(callingMethodInterceptors),
                        CalledInterceptors = new List<ICalledInterceptor>(calledMethodInterceptors),
                        ExceptionInterceptor = exceptionMethodInterceptor
                    };

                    foreach (var interfaceMethod in interfaceMethods)
                    {
                        if (method.Name == interfaceMethod.Name || method.Name == interfaceType.FullName + "." + interfaceMethod.Name)
                        {
                            var (callingInterfaceMethodInterceptors, calledInterfaceMethodInterceptors) =
                                GetMemberInterceptorsWithoutException(interfaceMethod);
                            wrapper.CallingInterceptors.AddRange(callingInterfaceMethodInterceptors);
                            wrapper.CalledInterceptors.AddRange(calledInterfaceMethodInterceptors);
                            if (wrapper.ExceptionInterceptor == null) // 接口方法的优先级较低
                            {
                                wrapper.ExceptionInterceptor = interfaceMethod.GetCustomAttribute<ExceptionInterceptAttribute>();
                            }

                            break;
                        }
                    }

                    _wrappers.Add(method.MetadataToken, wrapper);
                }
            }
        }

        public InterceptorWrapperCollection(Type classType, Type proxyType)
        {
            if (classType == null)
            {
                throw new ArgumentNullException(nameof(classType));
            }

            if (proxyType == null)
            {
                throw new ArgumentNullException(nameof(proxyType));
            }

            _wrappers = new Dictionary<int, InterceptorWrapper>();
            SetClassInterceptors(_wrappers, classType);
            var nonInterceptAttribute = typeof(NonInterceptAttribute);
            foreach (var methodHandle in HandleCollection.GetHandles(proxyType.MetadataToken))
            {
                var method = MethodBase.GetMethodFromHandle(methodHandle);
                if (!method.IsDefined(nonInterceptAttribute))
                {
                    var (callingMethodInterceptors, calledMethodInterceptors, exceptionInterceptor) = GetMemberInterceptors(method);
                    _wrappers.Add(method.MetadataToken, new InterceptorWrapper
                    {
                        CallingInterceptors = new List<ICallingInterceptor>(callingMethodInterceptors),
                        CalledInterceptors = new List<ICalledInterceptor>(calledMethodInterceptors),
                        ExceptionInterceptor = exceptionInterceptor
                    });
                }
            }
        }

        private (IEnumerable<ICallingInterceptor>, IEnumerable<ICalledInterceptor>, IExceptionInterceptor) GetMemberInterceptors(MemberInfo type)
        {
            var attributes = type.GetCustomAttributes<InterceptAttribute>(true);
            return
                (attributes
                  .OfType<ICallingInterceptor>()
                  .ToList(),
                attributes
                    .OfType<ICalledInterceptor>()
                    .ToList(),
                attributes
                    .OfType<IExceptionInterceptor>()
                    .FirstOrDefault());
        }


        private (IEnumerable<ICallingInterceptor>, IEnumerable<ICalledInterceptor>) GetMemberInterceptorsWithoutException(MemberInfo type)
        {
            var attributes = type.GetCustomAttributes<InterceptAttribute>(true);
            return
                (attributes
                  .OfType<ICallingInterceptor>()
                  .ToList(),
                attributes
                    .OfType<ICalledInterceptor>()
                    .ToList());
        }

        private void SetClassInterceptors(Dictionary<int, InterceptorWrapper> wrappers, Type classType)
        {
            var nonInterceptAttribute = typeof(NonInterceptAttribute);
            var globalInterceptors = GlobalInterceptorCollection.GlobalInterceptors;
            var globalWrapper = new InterceptorWrapper
            {
                CallingInterceptors = globalInterceptors.GetCallingInterceptors().ToList(),
                CalledInterceptors = globalInterceptors.GetCalledInterceptors().ToList(),
                ExceptionInterceptor = globalInterceptors.GetExceptionInterceptor()
            };

            _wrappers.Add(-1, globalWrapper);

            if (!classType.IsDefined(nonInterceptAttribute))
            {
                var (callingInterceptors, calledInterceptors, exceptionInterceptor) = GetMemberInterceptors(classType);
                var typeWrapper = new InterceptorWrapper
                {
                    CallingInterceptors = new List<ICallingInterceptor>(callingInterceptors),
                    CalledInterceptors = new List<ICalledInterceptor>(calledInterceptors),
                    ExceptionInterceptor = exceptionInterceptor
                };

                _wrappers.Add(0, typeWrapper);
            }
        }


        public InterceptorWrapper GetWrapper(int interfaceToken, int proxyToken)
        {
            var wrapper = new InterceptorWrapper();
            AppendWrapper(proxyToken, wrapper);
            AppendWrapper(interfaceToken, wrapper);
            AppendWrapper(0, wrapper);
            AppendWrapper(-1, wrapper);
            return wrapper;
        }

        public InterceptorWrapper GetWrapper(int proxyToken)
        {
            var wrapper = new InterceptorWrapper();
            AppendWrapper(proxyToken, wrapper);
            AppendWrapper(0, wrapper);
            AppendWrapper(-1, wrapper);
            return wrapper;
        }

        private void AppendWrapper(int token, InterceptorWrapper wrapper)
        {
            if (_wrappers.TryGetValue(token, out var value))
            {
                if (value != null)
                {
                    if (wrapper.CallingInterceptors == null)
                    {
                        wrapper.CallingInterceptors = new List<ICallingInterceptor>();
                    }

                    wrapper.CallingInterceptors.AddRange(value.CallingInterceptors);

                    if (wrapper.CalledInterceptors == null)
                    {
                        wrapper.CalledInterceptors = new List<ICalledInterceptor>();
                    }

                    wrapper.CalledInterceptors.AddRange(value.CalledInterceptors);

                    if (value.ExceptionInterceptor != null)
                    {
                        wrapper.ExceptionInterceptor = value.ExceptionInterceptor;
                    }
                }
            }
        }

        public IEnumerator<KeyValuePair<int, InterceptorWrapper>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<int, InterceptorWrapper>>)_wrappers).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<int, InterceptorWrapper>>)_wrappers).GetEnumerator();
        }
    }
}
