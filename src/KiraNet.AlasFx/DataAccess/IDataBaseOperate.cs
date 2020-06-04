using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace KiraNet.AlasFx.DataAccess
{
    public interface IDataBaseOperate<T>
    {
        T Insert(T model);
        T Insert(T model, object trans, IDbConnection conn);
        Task<T> InsertAsync(T model);
        Task<T> InsertAsync(T model, object trans, IDbConnection conn);
        T InsertUpdate(T model);
        T InsertUpdate(T model, object trans, IDbConnection conn);
        Task<T> InsertUpdateAsync(T model);
        Task<T> InsertUpdateAsync(T model, object trans, IDbConnection conn);
        int BatchInsert(IEnumerable<T> items);
        Task<int> BatchInsertAsync(IEnumerable<T> items);
        T Update(T model);
        T Update(T model, object trans, IDbConnection conn);
        Task<T> UpdateAsync(T model);
        Task<T> UpdateAsync(T model, object trans, IDbConnection conn);
        IEnumerable<T> GetList();
        Task<IEnumerable<T>> GetListAsync();
        T LoadModel(IDataReader dr);
        T LoadModel(DataRow dr);
        IEnumerable<T> LoadModel(DataTable dt);
    }
}
