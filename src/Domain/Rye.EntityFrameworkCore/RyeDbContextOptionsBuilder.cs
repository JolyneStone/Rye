using Microsoft.EntityFrameworkCore;
using Rye.EntityFrameworkCore.Options;
using System;
using System.Collections.Generic;

namespace Rye.EntityFrameworkCore
{
    public class RyeDbContextOptionsBuilder
    {
        internal Dictionary<Type, object> Options { get; } = new Dictionary<Type, object>();

        public RyeDbContextOptionsBuilder AddDbContext<TContext>(string dbName = null, Action<DbContextOptionsBuilder<TContext>> builderAction = null)
            where TContext: DbContext, IDbContext
        {
            var builder = new DbContextOptionsBuilder<TContext>();
            builderAction?.Invoke(builder);
            Options[typeof(TContext)] = (new DbContextOptionsBuilderOptions<TContext>(builder, dbName));
            return this;
        }
    }
}
