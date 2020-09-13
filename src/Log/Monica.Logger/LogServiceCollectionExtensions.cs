using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace Monica.Logger
{
    public static partial class LogServiceCollectionExtensions
    {
        /// <summary>
        /// 添加日志支持
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="useQueue">是否使用队列读写日志</param>
        /// <returns></returns>
        public static IServiceCollection AddMonicaLog(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }
            
            serviceCollection.AddLogging(builder => builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, MonicaLoggerProvider>()));
            return serviceCollection;
        }
    }
}
