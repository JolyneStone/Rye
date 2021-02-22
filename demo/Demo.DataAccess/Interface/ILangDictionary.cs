using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rye.DataAccess;

namespace Demo.DataAccess
{
    [Injection(ServiceLifetime.Scoped, InjectionPolicy.Replace)]
	public partial interface ILangDictionary : IDataBaseOperate<LangDictionary>
	{
		bool Delete(string dicKey,string lang);        
        bool Delete(string dicKey,string lang, IDbTransaction trans, IDbConnection conn);     
        Task<bool> DeleteAsync(string dicKey,string lang);
        Task<bool> DeleteAsync(string dicKey,string lang, IDbTransaction trans, IDbConnection conn);
        LangDictionary GetModel(string dicKey,string lang);
        LangDictionary GetModelByWriteDb(string dicKey,string lang);
        Task<LangDictionary> GetModelAsync(string dicKey,string lang);
        Task<LangDictionary> GetModelByWriteDbAsync(string dicKey,string lang);
        LangDictionary GetModel(string dicKey,string lang, IDbTransaction trans, IDbConnection conn);
        Task<LangDictionary> GetModelAsync(string dicKey,string lang, IDbTransaction trans, IDbConnection conn);
        bool Exists(string dicKey,string lang);
        bool ExistsByWriteDb(string dicKey,string lang);
        Task<bool> ExistsAsync(string dicKey,string lang);
        Task<bool> ExistsByWriteDbAsync(string dicKey,string lang);
        bool Exists(string dicKey,string lang, IDbTransaction trans, IDbConnection conn);
        Task<bool> ExistsAsync(string dicKey,string lang, IDbTransaction trans, IDbConnection conn);
	}
}
