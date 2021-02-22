using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;

namespace Rye.DataAccess
{
    //[Injection(ServiceLifetime.Scoped, InjectionPolicy.Replace)]
    public interface IConnectionProvider : IDisposable
    {
        IDbConnection GetDbConnection(string connectionString);
        IDbConnection GetDbConnectionByName(string connectionName);

        IDbConnection GetConnection();

        IDbConnection GetReadOnlyConnection();

        string GetConnectionString(string connectionName);

    }
}
