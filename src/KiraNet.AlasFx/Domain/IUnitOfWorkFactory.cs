using System;

namespace KiraNet.AlasFx.Domain
{
    /// <summary>
    /// UnitOfWork工厂接口
    /// </summary>
    public interface IUnitOfWorkFactory
    {
        /// <summary>
        /// 获取UnitOfWork接口对象
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        IUnitOfWork GetUnitOfWork(IServiceProvider serviceProvider, IDbContext context);
    }
}
