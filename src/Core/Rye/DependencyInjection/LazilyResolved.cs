using Microsoft.Extensions.DependencyInjection;

using System;

namespace Rye.DependencyInjection
{
    /// <summary>
    /// 用于解决循环依赖的问题
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LazilyResolved<T> : Lazy<T>
    {
        public LazilyResolved(IServiceProvider serviceProvider)
            : base(serviceProvider.GetRequiredService<T>)
        {
        }
    }
}
