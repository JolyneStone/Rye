using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;

namespace Monica.DataAccess
{
    [Injection(ServiceLifetime.Singleton, InjectionPolicy.Replace)]
    public interface IConnectionProvider : IDisposable
    {
        IDbConnection GetDbConnection(string connectionString);
        IDbConnection GetDbConnectionByName(string connectionName);

        IDbConnection GetConnection();

        IDbConnection GetReadOnlyConnection();

        string GetConnectionString(string connectionName);

    }
}
