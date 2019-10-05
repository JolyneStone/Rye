using Kira.AlasFx.Domain;
using Kira.AlasFx.EntityFrameworkCore.Options;
using Kira.AlasFx.Exceptions;
using Kira.AlasFx.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Kira.AlasFx.EntityFrameworkCore
{
    public class DbProvider : IDbProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, IUnitOfWork> _works = new Dictionary<string, IUnitOfWork>();
        private static ConcurrentDictionary<Type, Func<IServiceProvider, DbContextOptions, IDbContext>> _expressionFactoryDict =
            new ConcurrentDictionary<Type, Func<IServiceProvider, DbContextOptions, IDbContext>>();

        public DbProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IUnitOfWork GetUnitOfWork(Type dbContextType, string dbName = null)
        {
            Check.NotNull(dbContextType, nameof(dbContextType));
            // 若替换之前的UnitOfWork则有可能造成数据库连接释放不及时
            // 若使用锁，可以确保UnitOrWork在当前Scope生命周期的唯一性，但会影响性能
            var key = string.Format("{0}${1}$", dbName, dbContextType.FullName);
            if (_works.ContainsKey(key))
            {
                return _works[key];
            }
            else
            {
                foreach (var k in _works.Keys)
                {
                    if (k.StartsWith(key))
                    {
                        return _works[k];
                    }
                }

                IDbContext dbContext;
                key += DateTime.Now.Ticks.ToString();
                var dbConnectionOptionsMap = _serviceProvider.GetRequiredService<IOptions<AlasFx.Options.AlasFxOptions>>().Value.DbConnections;
                if (dbConnectionOptionsMap == null || dbConnectionOptionsMap.Count <= 0)
                {
                    throw new AlasFxException("无法获取数据库配置");
                }

                DbConnectionOptions dbConnectionOptions = dbName == null ? dbConnectionOptionsMap.First().Value : dbConnectionOptionsMap[dbName];

                var builderOptions = _serviceProvider.GetServices<DbContextOptionsBuilderOptions>()
                      ?.Where(d => d.DbName == dbName && (d.DbContextType == null || d.DbContextType == dbContextType))
                      ?.OrderByDescending(d => d.DbName)
                      ?.OrderByDescending(d => d.DbContextType);
                if (builderOptions == null || !builderOptions.Any())
                {
                    throw new AlasFxException("无法获取匹配的DbContextOptionsBuilder");
                }

                var dbUser = _serviceProvider.GetServices<IDbContextOptionsBuilderUser>()?.FirstOrDefault(u => u.Type == dbConnectionOptions.DatabaseType);
                if (dbUser == null)
                {
                    throw new AlasFxException($"无法解析类型为“{dbConnectionOptions.DatabaseType}”的 {typeof(IDbContextOptionsBuilderUser).FullName} 实例");
                }


                var dbContextOptions = dbUser.Use(builderOptions.First().Builder, dbConnectionOptions.ConnectionString).Options;
                if (_expressionFactoryDict.TryGetValue(dbContextType, out Func<IServiceProvider, DbContextOptions, IDbContext> factory))
                {
                    dbContext = factory(_serviceProvider, dbContextOptions);
                }
                else
                {
                    // 使用Expression创建DbContext
                    var constructorMethod = dbContextType.GetConstructors()
                        .Where(c => c.IsPublic && !c.IsAbstract && !c.IsStatic)
                        .OrderByDescending(c => c.GetParameters().Length)
                        .FirstOrDefault();
                    if (constructorMethod == null)
                    {
                        throw new AlasFxException("无法获取有效的上下文构造器");
                    }

                    var dbContextOptionsBuilderType = typeof(DbContextOptionsBuilder<>);
                    var dbContextOptionsType = typeof(DbContextOptions);
                    var dbContextOptionsGenericType = typeof(DbContextOptions<>);
                    var serviceProviderType = typeof(IServiceProvider);
                    var getServiceMethod = serviceProviderType.GetMethod("GetService");
                    var lambdaParameterExpressions = new ParameterExpression[2];
                    lambdaParameterExpressions[0] = (Expression.Parameter(serviceProviderType, "serviceProvider"));
                    lambdaParameterExpressions[1] = (Expression.Parameter(dbContextOptionsType, "dbContextOptions"));
                    var paramTypes = constructorMethod.GetParameters();
                    var argumentExpressions = new Expression[paramTypes.Length];
                    for (int i = 0; i < paramTypes.Length; i++)
                    {
                        var pType = paramTypes[i];
                        if (pType.ParameterType == dbContextOptionsType ||
                            (pType.ParameterType.IsGenericType && pType.ParameterType.GetGenericTypeDefinition() == dbContextOptionsGenericType))
                        {
                            argumentExpressions[i] = Expression.Convert(lambdaParameterExpressions[1], pType.ParameterType);
                        }
                        else if (pType.ParameterType == serviceProviderType)
                        {
                            argumentExpressions[i] = lambdaParameterExpressions[0];
                        }
                        else
                        {
                            argumentExpressions[i] = Expression.Call(lambdaParameterExpressions[0], getServiceMethod);
                        }
                    }

                    factory = Expression
                        .Lambda<Func<IServiceProvider, DbContextOptions, IDbContext>>(
                            Expression.Convert(Expression.New(constructorMethod, argumentExpressions), typeof(IDbContext)), lambdaParameterExpressions.AsEnumerable())
                        .Compile();
                    _expressionFactoryDict.TryAdd(dbContextType, factory);

                    dbContext = factory(_serviceProvider, dbContextOptions);
                }

                var unitOfWorkFactory = _serviceProvider.GetRequiredService<IUnitOfWorkFactory>();
                var unitOfWork = unitOfWorkFactory.GetUnitOfWork(_serviceProvider, dbContext);
                _works.Add(key, unitOfWork);
                return unitOfWork;
            }
        }

        public void Dispose()
        {
            if (_works != null && _works.Count > 0)
            {
                foreach (var unitOfWork in _works.Values)
                    unitOfWork.Dispose();
                _works.Clear();
            }
        }
    }
}
