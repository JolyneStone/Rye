using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Monica.DataAccess.MySql.Model
{
    [Injection(ServiceLifetime.Singleton, InjectionPolicy.Replace)]
	public interface IExternNews : IDataBaseOperate<ExternNews>
	{
		bool Delete(long id);        
        bool Delete(long id, IDbTransaction trans, IDbConnection conn);     
        Task<bool> DeleteAsync(long id);
        Task<bool> DeleteAsync(long id, IDbTransaction trans, IDbConnection conn);
        ExternNews GetModel(long id);
        ExternNews GetModelByWriteDb(long id);
        Task<ExternNews> GetModelAsync(long id);
        Task<ExternNews> GetModelByWriteDbAsync(long id);
        ExternNews GetModel(long id, IDbTransaction trans, IDbConnection conn);
        Task<ExternNews> GetModelAsync(long id, IDbTransaction trans, IDbConnection conn);
        bool Exists(long id);
        bool ExistsByWriteDb(long id);
        Task<bool> ExistsAsync(long id);
        Task<bool> ExistsByWriteDbAsync(long id);
        bool Exists(long id, IDbTransaction trans, IDbConnection conn);
        Task<bool> ExistsAsync(long id, IDbTransaction trans, IDbConnection conn);
		long GetLastIdentity();
	}
}
