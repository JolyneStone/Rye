using KiraNet.AlasFx.Domain;
using System;

namespace KiraNet.AlasFx.EntityFrameworkCore
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork GetUnitOfWork(IServiceProvider serviceProvider, IDbContext context)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));
            Check.NotNull(context, nameof(context));
            return new UnitOfWork(serviceProvider, context);
        }
    }
}
