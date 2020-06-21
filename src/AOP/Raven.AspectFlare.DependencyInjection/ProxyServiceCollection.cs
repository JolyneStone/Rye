using System;
using System.Collections;
using System.Collections.Generic;
using Raven.AspectFlare.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace Raven.AspectFlare.DependencyInjection
{
    public class ProxyServiceCollection : IServiceCollection
    {
        private readonly IServiceCollection _services;
        private readonly IProxyProvider _provider;

        public ProxyServiceCollection(IServiceCollection services, IProxyFlare proxyFlare)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _provider = proxyFlare.GetProvider();
        }

        public ServiceDescriptor this[int index] { get => _services[index]; set => _services[index] = value; }

        public int Count => _services.Count;

        public bool IsReadOnly => _services.IsReadOnly;

        public void Add(ServiceDescriptor item)
        {
            // 目前我还不能实现对已实例化的对象进行动态代理，因此直接注入
            if (item.ImplementationInstance != null && item.ImplementationFactory == null)
            {
                _services.Add(item);
                return;
            }

            Type classType = item.ImplementationType ??
                            item.ImplementationInstance?.GetType() ??
                            item.ImplementationFactory?.GetType().GetGenericArguments()[1] ??
                            null;

            IExpressionConverter<IServiceProvider, object> expressionConvertr = new ServiceProviderExpressionConverter();
            Type implementType;
            if (item.ServiceType.IsInterface)
            {
                implementType = _provider.GetProxyType(item.ServiceType, classType);
            }
            else
            {
                implementType = _provider.GetProxyType(classType);
            }

            if (implementType == null)
            {
                _services.Add(item);
                return;
            }

            if (item.ImplementationFactory == null)
            {
                _services.Add(ServiceDescriptor.Describe(
                                item.ServiceType,
                                implementType,
                                item.Lifetime));
            }
            else if (expressionConvertr.TryConvert(service =>
                                item.ImplementationFactory(service),
                                item.ImplementationType,
                                implementType,
                                out var convertLambdaExpression))
            {
                _services.Add(ServiceDescriptor.Describe(
                                item.ServiceType,
                                convertLambdaExpression.Compile(),
                                item.Lifetime));
            }
        }

        public void Clear()
        {
            _services.Clear();
        }

        public bool Contains(ServiceDescriptor item)
        {
            return _services.Contains(item);
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            _services.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return _services.GetEnumerator();
        }

        public int IndexOf(ServiceDescriptor item)
        {
            return _services.IndexOf(item);
        }

        public void Insert(int index, ServiceDescriptor item)
        {
            _services.Insert(index, item);
        }

        public bool Remove(ServiceDescriptor item)
        {
            return _services.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _services.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _services.GetEnumerator();
        }
    }
}
