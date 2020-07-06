using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Raven.DataAccess
{
    public interface IDataBaseOperate<T>
    {
        T Insert(T model);
        T Insert(T model, IDbTransaction trans, IDbConnection conn);
        Task<int> InsertAsync(T model);
        Task<int> InsertAsync(T model, IDbTransaction trans, IDbConnection conn);
        Task<int> InsertModelAsync(T model);
        Task<int> InsertModelAsync(T model, IDbTransaction trans, IDbConnection conn);
        Task<T> InsertUpdateModelAsync(T model);
        Task<T> InsertUpdateModelAsync(T model, IDbTransaction trans, IDbConnection conn);
        T InsertUpdate(T model);
        T InsertUpdate(T model, IDbTransaction trans, IDbConnection conn);
        Task<T> InsertUpdateAsync(T model);
        Task<T> InsertUpdateAsync(T model, IDbTransaction trans, IDbConnection conn);
        int BatchInsert(IEnumerable<T> items);
        Task<int> BatchInsertAsync(IEnumerable<T> items);
        T Update(T model);
        T Update(T model, IDbTransaction trans, IDbConnection conn);
        Task<T> UpdateAsync(T model);
        Task<T> UpdateAsync(T model, IDbTransaction trans, IDbConnection conn);
        IEnumerable<T> GetList();
        Task<IEnumerable<T>> GetListAsync();
        IEnumerable<T> GetList(string whereSql, string orderSql, int pageIndex, int pageSize);
        Task<IEnumerable<T>> GetListAsync(string whereSql, string orderSql, int pageIndex, int pageSize);
        T LoadModel(IDataReader dr);
        T LoadModel(DataRow dr);
        IEnumerable<T> LoadModel(DataTable dt);
    }
}
