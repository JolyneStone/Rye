using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Raven.DataAccess.Model
{
    [Injection(ServiceLifetime.Singleton, InjectionPolicy.Replace, typeof(DaoTest1))]

    public interface ITest1 : IDataBaseOperate<Test1>

    {
        bool Delete(int id);

        bool Delete(int id, object trans, IDbConnection conn);

        Task<bool> DeleteAsync(int id);

        Task<bool> DeleteAsync(int id, object trans, IDbConnection conn);


        Test1 GetModel(int id);

        Task<Test1> GetModelAsync(int id);


        bool Exists(int id);

        Task<bool> ExistsAsync(int id);

    }
}
