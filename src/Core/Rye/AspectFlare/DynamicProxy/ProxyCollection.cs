using System;
using System.Collections.Generic;

namespace Rye.AspectFlare.DynamicProxy
{
    public class ProxyCollection : IProxyCollection
    {
        private readonly TypeDictionary<Type> _proxyClassContainer = new TypeDictionary<Type>();
        private readonly TypeDictionary<TypeDictionary<Type>> _proxyInterfaceContainer = new TypeDictionary<TypeDictionary<Type>>();

        public void Add(Type interfaceType, Type classType, Type proxyType)
        {
            if (classType == null)
            {
                throw new ArgumentNullException(nameof(classType));
            }

            if (proxyType == null)
            {
                throw new ArgumentNullException(nameof(proxyType));
            }

            if (interfaceType == null)
            {
                _proxyClassContainer.AddValue(classType, proxyType);
            }
            else
            {
                var dict = _proxyInterfaceContainer.GetValue(interfaceType);
                if (dict == null)
                {
                    var tmp = new TypeDictionary<Type>();
                    tmp.AddValue(classType, proxyType);
                    _proxyInterfaceContainer.AddValue(interfaceType, tmp);
                }
                else
                {
                    dict.AddValue(classType, proxyType);
                }
            }
        }

        public Type GetProxyType(Type interfaceType, Type classType)
        {
            if (classType == null)
            {
                throw new ArgumentNullException(nameof(classType));
            }

            if (interfaceType == null)
            {
                return _proxyClassContainer.GetValue(classType);
            }
            else
            {
                var dict = _proxyInterfaceContainer.GetValue(interfaceType);
                if (dict != null)
                {
                    return dict.GetValue(classType);
                }
            }

            return null;
        }

        private class TypeDictionary<TValue>
            where TValue : class
        {
            private object _sync = new object();
            private Dictionary<Type, TValue> _dict = new Dictionary<Type, TValue>();

            public TValue GetValue(Type type)
            {
                if (type == null)
                {
                    throw new ArgumentNullException(nameof(type));
                }

                if (_dict.TryGetValue(type, out var value))
                {
                    return value;
                }

                return null;
            }

            public void AddValue(Type type, TValue value)
            {
                if (type == null)
                {
                    throw new ArgumentNullException(nameof(type));
                }

                if (_dict.ContainsKey(type))
                {
                    return;
                }

                lock (_sync)
                {
                    _dict.Add(type, value);
                }
            }
        }
    }
}
