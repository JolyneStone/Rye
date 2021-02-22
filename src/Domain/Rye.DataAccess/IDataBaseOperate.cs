using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Rye.DataAccess
{
    public interface IDataBaseOperate<T>
    {
        int Insert(T model);
        int Insert(T model, IDbTransaction trans, IDbConnection conn);
        Task<int> InsertAsync(T model);
        Task<int> InsertAsync(T model, IDbTransaction trans, IDbConnection conn);
        int InsertUpdate(T model);
        int InsertUpdate(T model, IDbTransaction trans, IDbConnection conn);
        Task<int> InsertUpdateAsync(T model);
        Task<int> InsertUpdateAsync(T model, IDbTransaction trans, IDbConnection conn);
        int BatchInsert(IEnumerable<T> items);
        Task<int> BatchInsertAsync(IEnumerable<T> items);
        int BatchInsert(IEnumerable<T> items, IDbTransaction trans, IDbConnection conn);
        Task<int> BatchInsertAsync(IEnumerable<T> items, IDbTransaction trans, IDbConnection conn);
        int Update(T model);
        int Update(T model, IDbTransaction trans, IDbConnection conn);
        Task<int> UpdateAsync(T model);
        Task<int> UpdateAsync(T model, IDbTransaction trans, IDbConnection conn);
        IEnumerable<T> GetList();
        Task<IEnumerable<T>> GetListAsync();
        IEnumerable<T> GetList(IDbTransaction trans, IDbConnection conn);
        Task<IEnumerable<T>> GetListAsync(IDbTransaction trans, IDbConnection conn);
        IEnumerable<T> GetListByWriteDb();
        Task<IEnumerable<T>> GetListByWriteDbAsync();
        IEnumerable<T> GetListByWriteDb(IDbTransaction trans, IDbConnection conn);
        Task<IEnumerable<T>> GetListByWriteDbAsync(IDbTransaction trans, IDbConnection conn);
        IEnumerable<T> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize);
        Task<IEnumerable<T>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize);
        IEnumerable<T> GetPageByWriteDb(object param, string whereSql, string orderSql, int pageIndex, int pageSize);
        Task<IEnumerable<T>> GetPageByWriteDbAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize);
        IEnumerable<T> GetPage(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn);
        Task<IEnumerable<T>> GetPageAsync(object param, string whereSql, string orderSql, int pageIndex, int pageSize, IDbTransaction trans, IDbConnection conn);
        bool Exists(object param, string whereSql, IDbTransaction trans, IDbConnection conn);
        Task<bool> ExistsAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn);
        bool Exists(object param, string whereSql);
        Task<bool> ExistsAsync(object param, string whereSql);
        bool ExistsByWriteDb(object param, string whereSql);
        Task<bool> ExistsByWriteDbAsync(object param, string whereSql);
        int Count();
        Task<int> CountAsync();
        int CountByWriteDb();
        Task<int> CountByWriteDbAsync();
        int Count(object param, string whereSql);
        Task<int> CountAsync(object param, string whereSql);
        int CountByWriteDb(object param, string whereSql);
        Task<int> CountByWriteDbAsync(object param, string whereSql);
        int Count(IDbTransaction trans, IDbConnection conn);
        Task<int> CountAsync(IDbTransaction trans, IDbConnection conn);
        int Count(object param, string whereSql, IDbTransaction trans, IDbConnection conn);
        Task<int> CountAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn);
        T GetModel(object param, string whereSql);
        Task<T> GetModelAsync(object param, string whereSql);
        T GetModelByWriteDb(object param, string whereSql);

        Task<T> GetModelByWriteDbAsync(object param, string whereSql);
        T GetModel(object param, string whereSql, IDbTransaction trans, IDbConnection conn);
        Task<T> GetModelAsync(object param, string whereSql, IDbTransaction trans, IDbConnection conn);
        T FirstOrDefault(object param, string whereSql, string orderSql);
        Task<T> FirstOrDefaultAsync(object param, string whereSql, string orderSql);
        T FirstOrDefaultByWriteDb(object param, string whereSql, string orderSql);
        Task<T> FirstOrDefaultByWriteDbAsync(object param, string whereSql, string orderSql);
        T FirstOrDefault(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn);
        Task<T> FirstOrDefaultAsync(object param, string whereSql, string orderSql, IDbTransaction trans, IDbConnection conn);
    }
}
