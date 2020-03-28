using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace KiraNet.AlasFx.DependencyInjection
{
    /// <summary>
    /// 全局单例服务类
    /// </summary>
    public class SingleServiceLocator
    {
        private static readonly object ObjLock = new object();

        private static IServiceProvider _serviceProvider;
        /// <summary>
        /// 
        /// </summary>
        public static IServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    lock (ObjLock)
                    {
                        if (_serviceProvider == null)
                        {
                            _serviceProvider = Instance.Build();
                        }
                    }
                }
                return _serviceProvider;
            }
            set => _serviceProvider = value;
        }

        private static readonly ServiceLocator Instance = new ServiceLocator();

        // Methods
        public static void ConfigService(Action<IServiceCollection> configFun)
        {
            if (configFun != null)
            {
                if (Instance.Services == null)
                {
                    lock (ObjLock)
                    {
                        if (Instance.Services == null)
                        {
                            Instance.Services = new ServiceCollection();
                        }
                    }
                }
                Instance.ConfigureServices(configFun);
                ServiceProvider = Instance.Build();
            }
        }

        /// <summary>
        /// 设置 ServiceCollection 对象，
        /// 注：会覆盖掉原来注入的对象，建议项目只调用一次，之后需要动态注入可使用 ConfigService 方法
        /// </summary>
        /// <param name="services"></param>
        public static void SetServiceCollection(IServiceCollection services)
        {
            if (services == null)
            {
                return;
            }

            lock (ObjLock)
            {
                Instance.SetServiceCollection(services);
                ServiceProvider = Instance.Build();
            }
        }

        /// <summary>
        /// 获取服务，服务未注入会抛异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isRequired">是否必须。为true时，没注入会抛异常； 为false时，没注入返回null</param>
        /// <returns></returns>
        public static T GetService<T>(bool isRequired = false)
        {
            if (isRequired)
            {
                return ServiceProvider.GetRequiredService<T>();
            }
            else
            {
                return ServiceProvider.GetService<T>();
            }
        }

        internal class ServiceLocator
        {
            public IServiceCollection Services { get; set; }

            public IServiceProvider Build()
            {
                return Services.BuildServiceProvider();
            }

            public void ConfigureServices(Action<IServiceCollection> configFun)
            {
                configFun?.Invoke(Services);
            }

            public void SetServiceCollection(IServiceCollection serviceCollection)
            {
                Services = serviceCollection;
            }

        }
    }
}
