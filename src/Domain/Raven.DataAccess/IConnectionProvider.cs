using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Raven.DataAccess
{
    [Injection(ServiceLifetime.Singleton, InjectionPolicy.Replace)]
    public interface IConnectionProvider
    {
        IDbConnection GetDbConnection(string connectionString, bool open = true);

        IDbConnection GetConnection(bool open = true);

        IDbConnection GetReadOnlyConnection(bool open = true);

        string GetConnectionString(string connectionName);

    }
}
