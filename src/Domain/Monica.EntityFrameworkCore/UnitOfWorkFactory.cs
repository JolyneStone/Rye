using Monica.DataAccess;
using System;

namespace Monica.EntityFrameworkCore
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
