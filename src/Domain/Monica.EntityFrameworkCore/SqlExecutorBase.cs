using Dapper;
using Microsoft.EntityFrameworkCore;
using Monica.DataAccess;
using Monica.Enums;
using System;
using System.Collections.Generic;
using System.Data;

namespace Monica.EntityFrameworkCore
{
    public abstract class SqlExecutorBase : ISqlExecutor
    {
        private readonly string _connectionString;
        protected IDbConnection _conn;

        public SqlExecutorBase(IUnitOfWork unitOfWork)
        {
            if (unitOfWork is null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }

            _connectionString = unitOfWork.DbContext.AsDbContext().Database.GetDbConnection().ConnectionString;
        }

        public SqlExecutorBase(IDbContext dbContext)
        {
            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _connectionString = dbContext.AsDbContext().Database.GetDbConnection().ConnectionString;
        }

        public SqlExecutorBase(DbContext dbContext)
        {
            _connectionString = dbContext.Database.GetDbConnection().ConnectionString;
        }

        public SqlExecutorBase(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("message", nameof(connectionString));
            }

            _connectionString = connectionString;
        }

        protected abstract IDbConnection GetDbConnection(string connectionString);

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        public abstract DatabaseType DatabaseType { get; }

        /// <summary>
        /// 执行指定的SQL语句
        /// </summary>
        /// <param name="sql">执行的SQL语句</param>
        /// <param name="param">SQL参数</param>
        /// <returns>操作影响的行数</returns>
        public virtual int ExecuteSqlCommand(string sql, object param = null)
        {
            if (_conn == null)
                _conn = GetDbConnection(_connectionString);
            if (_conn.State != ConnectionState.Open)
                _conn.Open();

            return _conn.Execute(sql, param);
        }

        /// <summary>
        /// 查询指定SQL的结果集
        /// </summary>
        /// <typeparam name="TResult">结果集类型</typeparam>
        /// <param name="sql">查询的SQL语句</param>
        /// <param name="param">SQL参数</param>
        /// <returns>结果集</returns>
        public virtual IEnumerable<TResult> FromSql<TResult>(string sql, object param = null)
        {
            if (_conn == null)
                _conn = GetDbConnection(_connectionString);
            if (_conn.State != ConnectionState.Open)
                _conn.Open();

            return _conn.Query<TResult>(sql, param);
        }

        public virtual ITransaction BeginTransaction(IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            if (_conn == null)
            {
                throw new ArgumentNullException(nameof(_conn));
            }

            if (_conn.State != ConnectionState.Open)
                _conn.Open();
            return new EFCoreTransaction(_conn.BeginTransaction(isolation));
        }

        public void Dispose()
        {
            _conn?.Dispose();
        }
    }
}
