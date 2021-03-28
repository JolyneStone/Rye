using Rye.DataAccess;
using Rye.EntityFrameworkCore.Options;
using Rye.Exceptions;
using Rye.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Rye.DataAccess.Options;

namespace Rye.EntityFrameworkCore
{
    public class DbProvider : IDbProvider
    {
        private readonly Random _random = new Random();
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, IUnitOfWork> _works = new Dictionary<string, IUnitOfWork>();
        //private static readonly ConcurrentDictionary<Type, Func<IServiceProvider, DbContextOptions, IDbContext>> _expressionFactoryDict =
        //    new ConcurrentDictionary<Type, Func<IServiceProvider, DbContextOptions, IDbContext>>();

        public DbProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        //public IUnitOfWork GetUnitOfWork(Type dbContextType, string dbName = null)
        //{
        //    Check.NotNull(dbContextType, nameof(dbContextType));
        //    // 若替换之前的UnitOfWork则有可能造成数据库连接释放不及时
        //    // 若使用锁，可以确保UnitOrWork在当前Scope生命周期的唯一性，但会影响性能
        //    var key = string.Format("{0}${1}$", dbName, dbContextType.FullName);
        //    if (_works.ContainsKey(key))
        //    {
        //        return _works[key];
        //    }
        //    else
        //    {
        //        foreach (var k in _works.Keys)
        //        {
        //            if (k.StartsWith(key))
        //            {
        //                return _works[k];
        //            }
        //        }

        //        IDbContext dbContext;
        //        key += DateTime.Now.Ticks.ToString();
        //        var dbConnectionOptionsMap = _serviceProvider.GetRequiredService<IOptions<DbConnectionMapOptions>>().Value;
        //        if (dbConnectionOptionsMap == null || dbConnectionOptionsMap.Count <= 0)
        //        {
        //            throw new RyeException("无法获取数据库配置");
        //        }

        //        DbConnectionOptions dbConnectionOptions = dbName == null ? dbConnectionOptionsMap.First().Value : dbConnectionOptionsMap[dbName];

        //        var builderOptions = _serviceProvider.GetServices<DbContextOptionsBuilderOptions>()
        //              ?.Where(d => (dbName == null || d.DbName == null || d.DbName == dbName) && (d.DbContextType == null || d.DbContextType == dbContextType))
        //              ?.OrderByDescending(d => d.DbName)
        //              ?.OrderByDescending(d => d.DbContextType);
        //        if (builderOptions == null || !builderOptions.Any())
        //        {
        //            throw new RyeException("无法获取匹配的DbContextOptionsBuilder");
        //        }

        //        var dbUser = _serviceProvider.GetServices<IDbContextOptionsBuilderUser>()?.FirstOrDefault(u => u.Type == dbConnectionOptions.DatabaseType);
        //        if (dbUser == null)
        //        {
        //            throw new RyeException($"无法解析类型为“{dbConnectionOptions.DatabaseType}”的 {typeof(IDbContextOptionsBuilderUser).FullName} 实例");
        //        }


        //        var dbContextOptions = dbUser.Use(builderOptions.First().Builder, dbConnectionOptions.ConnectionString).Options;
        //        if (_expressionFactoryDict.TryGetValue(dbContextType, out Func<IServiceProvider, DbContextOptions, IDbContext> factory))
        //        {
        //            dbContext = factory(_serviceProvider, dbContextOptions);
        //        }
        //        else
        //        {
        //            // 使用Expression创建DbContext
        //            var constructorMethod = dbContextType.GetConstructors()
        //                .Where(c => c.IsPublic && !c.IsAbstract && !c.IsStatic)
        //                .OrderByDescending(c => c.GetParameters().Length)
        //                .FirstOrDefault();
        //            if (constructorMethod == null)
        //            {
        //                throw new RyeException("无法获取有效的上下文构造器");
        //            }

        //            var dbContextOptionsBuilderType = typeof(DbContextOptionsBuilder<>);
        //            var dbContextOptionsType = typeof(DbContextOptions);
        //            var dbContextOptionsGenericType = typeof(DbContextOptions<>);
        //            var serviceProviderType = typeof(IServiceProvider);
        //            var getServiceMethod = serviceProviderType.GetMethod("GetService");
        //            var lambdaParameterExpressions = new ParameterExpression[2];
        //            lambdaParameterExpressions[0] = (Expression.Parameter(serviceProviderType, "serviceProvider"));
        //            lambdaParameterExpressions[1] = (Expression.Parameter(dbContextOptionsType, "dbContextOptions"));
        //            var paramTypes = constructorMethod.GetParameters();
        //            var argumentExpressions = new Expression[paramTypes.Length];
        //            for (int i = 0; i < paramTypes.Length; i++)
        //            {
        //                var pType = paramTypes[i];
        //                if (pType.ParameterType == dbContextOptionsType ||
        //                    (pType.ParameterType.IsGenericType && pType.ParameterType.GetGenericTypeDefinition() == dbContextOptionsGenericType))
        //                {
        //                    argumentExpressions[i] = Expression.Convert(lambdaParameterExpressions[1], pType.ParameterType);
        //                }
        //                else if (pType.ParameterType == serviceProviderType)
        //                {
        //                    argumentExpressions[i] = lambdaParameterExpressions[0];
        //                }
        //                else
        //                {
        //                    argumentExpressions[i] = Expression.Call(lambdaParameterExpressions[0], getServiceMethod);
        //                }
        //            }

        //            factory = Expression
        //                .Lambda<Func<IServiceProvider, DbContextOptions, IDbContext>>(
        //                    Expression.Convert(Expression.New(constructorMethod, argumentExpressions), typeof(IDbContext)), lambdaParameterExpressions.AsEnumerable())
        //                .Compile();
        //            _expressionFactoryDict.TryAdd(dbContextType, factory);

        //            dbContext = factory(_serviceProvider, dbContextOptions);
        //        }

        //        var unitOfWorkFactory = _serviceProvider.GetRequiredService<IUnitOfWorkFactory>();
        //        var unitOfWork = unitOfWorkFactory.GetUnitOfWork(_serviceProvider, dbContext);
        //        _works.Add(key, unitOfWork);
        //        return unitOfWork;
        //    }
        //}

        public IUnitOfWork GetUnitOfWork<TDbContext>(string dbName = null)
            where TDbContext : DbContext, IDbContext
        {
            var dbContextType = typeof(TDbContext);
            var key = string.Format("{0}${1}$", dbName, dbContextType.FullName);
            foreach (var k in _works.Keys)
            {
                if (k.StartsWith(key))
                {
                    return _works[k];
                }
            }

            var unitOfWork = InternalDbProvider<TDbContext>.GetUnitOfWork(_serviceProvider, dbName);
            _works.Add($"{key}{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}{_random.Next(1000)}", unitOfWork);
            return unitOfWork;
        }

        private class InternalDbProvider<TDbContext>
            where TDbContext : DbContext, IDbContext
        {
            private static Func<IServiceProvider, DbContextOptions<TDbContext>, IDbContext> _factory;

            public static IUnitOfWork GetUnitOfWork(IServiceProvider serviceProvider, string dbName = null)
            {
                var dbContextType = typeof(TDbContext);
                IDbContext dbContextInstance;
                var dbConnectionOptionsMap = serviceProvider.GetRequiredService<IOptions<DbConnectionMapOptions>>().Value;
                if (dbConnectionOptionsMap == null || dbConnectionOptionsMap.Count <= 0)
                {
                    throw new RyeException("无法获取数据库配置");
                }

                DbConnectionOptions dbConnectionOptions = dbName == null ? dbConnectionOptionsMap.First().Value : dbConnectionOptionsMap[dbName];

                var builderOptions = serviceProvider.GetServices<DbContextOptionsBuilderOptions<TDbContext>>()
                      ?.OrderByDescending(d => d.DbName == dbName)
                      ?.ThenBy(d => d.DbName);
                if (builderOptions == null || !builderOptions.Any())
                {
                    throw new RyeException("无法获取匹配的DbContextOptionsBuilder");
                }

                var dbUser = serviceProvider.GetServices<IDbContextOptionsBuilderUser>()?.FirstOrDefault(u => u.Type == dbConnectionOptions.DatabaseType);
                if (dbUser == null)
                {
                    throw new RyeException($"无法解析类型为“{dbConnectionOptions.DatabaseType}”的 {typeof(IDbContextOptionsBuilderUser).FullName} 实例");
                }

                var builder = builderOptions.First().Builder;
                var dbContextOptions = dbUser.Use(builder, dbConnectionOptions.ConnectionString).Options;
                if (_factory == null)
                {
                    // 使用Expression创建DbContext
                    var constructorMethods = dbContextType.GetConstructors()
                        .Where(c => c.IsPublic && !c.IsAbstract && !c.IsStatic)
                        .OrderByDescending(c => c.GetParameters().Length);
                    if (constructorMethods == null || !constructorMethods.Any())
                    {
                        throw new RyeException("无上下文构造器");
                    }

                    var dbContextOptionsType = typeof(DbContextOptions<TDbContext>);
                    var serviceProviderType = typeof(IServiceProvider);
                    var getServiceMethod = serviceProviderType.GetMethod("GetService");
                    var lambdaParameterExpressions = new ParameterExpression[2];
                    lambdaParameterExpressions[0] = (Expression.Parameter(serviceProviderType, "serviceProvider"));
                    lambdaParameterExpressions[1] = (Expression.Parameter(dbContextOptionsType, "dbContextOptions"));

                    foreach (var constructorMethod in constructorMethods)
                    {
                        var paramTypes = constructorMethod.GetParameters();
                        var argumentExpressions = new Expression[paramTypes.Length];
                        var hasDbContextOptionsType = false;
                        for (int i = 0; i < paramTypes.Length; i++)
                        {
                            var pType = paramTypes[i];
                            if (pType.ParameterType == dbContextOptionsType)
                            {
                                argumentExpressions[i] = Expression.Convert(lambdaParameterExpressions[1], pType.ParameterType);
                                hasDbContextOptionsType = true;
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
                        if (!hasDbContextOptionsType)
                        {
                            break;
                        }
                        _factory = Expression
                            .Lambda<Func<IServiceProvider, DbContextOptions<TDbContext>, IDbContext>>(
                                Expression.Convert(Expression.New(constructorMethod, argumentExpressions), typeof(IDbContext)), lambdaParameterExpressions.AsEnumerable())
                            .Compile();
                        break;
                    }

                    if (_factory == null)
                    {
                        throw new RyeException("无法获取有效的上下文构造器");
                    }
                }
                dbContextInstance = _factory(serviceProvider, dbContextOptions);
                var unitOfWorkFactory = serviceProvider.GetRequiredService<IUnitOfWorkFactory>();
                var unitOfWork = unitOfWorkFactory.GetUnitOfWork(serviceProvider, dbContextInstance);
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
