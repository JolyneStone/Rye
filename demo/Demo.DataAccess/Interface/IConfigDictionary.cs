using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rye.DataAccess;

namespace Demo.DataAccess
{
    [Injection(ServiceLifetime.Singleton, InjectionPolicy.Replace)]
	public partial interface IConfigDictionary : IDataBaseOperate<ConfigDictionary>
	{
		bool Delete(string dicKey);        
        bool Delete(string dicKey, IDbTransaction trans, IDbConnection conn);     
        Task<bool> DeleteAsync(string dicKey);
        Task<bool> DeleteAsync(string dicKey, IDbTransaction trans, IDbConnection conn);
        ConfigDictionary GetModel(string dicKey);
        ConfigDictionary GetModelByWriteDb(string dicKey);
        Task<ConfigDictionary> GetModelAsync(string dicKey);
        Task<ConfigDictionary> GetModelByWriteDbAsync(string dicKey);
        ConfigDictionary GetModel(string dicKey, IDbTransaction trans, IDbConnection conn);
        Task<ConfigDictionary> GetModelAsync(string dicKey, IDbTransaction trans, IDbConnection conn);
        bool Exists(string dicKey);
        bool ExistsByWriteDb(string dicKey);
        Task<bool> ExistsAsync(string dicKey);
        Task<bool> ExistsByWriteDbAsync(string dicKey);
        bool Exists(string dicKey, IDbTransaction trans, IDbConnection conn);
        Task<bool> ExistsAsync(string dicKey, IDbTransaction trans, IDbConnection conn);
	}
}
