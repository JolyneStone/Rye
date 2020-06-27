using Raven.DataAccess;
using Raven.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Raven
{
    /// <summary>
    /// 数据库上下文扩展方法
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PageData<IOutput> GetPageData<TOutput>(this IQueryable<IOutput> queryable, int pageIndex, int pageSize)
            where TOutput : class, IOutput
        {
            Check.NotNull(queryable, nameof(queryable));
            var pageQuery = queryable.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var pageData = new PageData<IOutput>
            {
                Total = queryable.Count(),
                Data = queryable.ToList()
            };
            return pageData;
        }

        /// <summary>
        /// 异步获取分页数据
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PageData<IOutput>> GetPageDataAsync<TOutput>(this IQueryable<IOutput> queryable, int pageIndex, int pageSize)
            where TOutput : class, IOutput
        {
            Check.NotNull(queryable, nameof(queryable));
            var pageQuery = queryable.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var pageData = new PageData<IOutput>
            {
                Total = await queryable.CountAsync(),
                Data = await queryable.ToListAsync()
            };
            return pageData;
        }

        /// <summary>
        /// 更新上下文中指定实体的状态
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context"></param>
        /// <param name="entities"></param>
        public static void Update<TEntity>(this DbContext context, params TEntity[] entities)
            where TEntity: class, IEntity
        {
            Check.NotNull(context, nameof(context));
            Check.NotNull(entities, nameof(entities));
            context.Set<TEntity>().BulkUpdate(entities);
        }

        /// <summary>
        /// 转换为DbContext
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static DbContext AsDbContext(this IDbContext context)
        {
            return context as DbContext;
        }

        /// <summary>
        /// 当前上下文是否是关系型数据库
        /// </summary>
        public static bool IsRelationalTransaction(this DbContext context)
        {
            return context.Database.GetService<IDbContextTransactionManager>() is IRelationalTransactionManager;
        }

        /// <summary>
        /// 检测关系型数据库是否存在
        /// </summary>
        public static bool ExistsRelationalDatabase(this DbContext context)
        {
            RelationalDatabaseCreator creator = context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            return creator != null && creator.Exists();
        }

        /// <summary>
        /// 获取未提交的迁移记录并提交迁移
        /// </summary>
        public static void CheckAndMigration(this DbContext dbContext)
        {
            string[] migrations = dbContext.Database.GetPendingMigrations().ToArray();
            if (migrations.Length > 0)
            {
                dbContext.Database.Migrate();
                ILoggerFactory loggerFactory = dbContext.GetService<ILoggerFactory>();
                ILogger logger = loggerFactory.CreateLogger(typeof(DbContextExtensions));
                logger.LogInformation($"已提交{migrations.Length}条挂起的迁移记录：{string.Join(",", migrations)}");
            }
        }

        /// <summary>
        /// 执行指定的Sql语句
        /// </summary>
        public static int ExecuteSqlCommand(this IDbContext dbContext, string sql, params object[] parameters)
        {
            if (!(dbContext is DbContext context))
            {
                throw new RavenException($"参数dbContext类型为“{dbContext.GetType()}”，不能转换为 DbContext");
            }
            return context.Database.ExecuteSqlRaw(sql, parameters);
        }

        /// <summary>
        /// 异步执行指定的Sql语句
        /// </summary>
        public static Task<int> ExecuteSqlCommandAsync(this IDbContext dbContext, string sql, params object[] parameters)
        {
            if (!(dbContext is DbContext context))
            {
                throw new RavenException($"参数dbContext类型为“{dbContext.GetType()}”，不能转换为 DbContext");
            }
            return context.Database.ExecuteSqlRawAsync(sql, parameters);
        }
    }
}
