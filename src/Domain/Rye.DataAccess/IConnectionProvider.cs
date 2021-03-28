using Microsoft.Extensions.DependencyInjection;

using Rye.DataAccess.Pool;

using System;
using System.Data;

namespace Rye.DataAccess
{
    public interface IConnectionProvider
    {
        Connector GetDbConnection(string connectionString, bool usePool = true);
        Connector GetDbConnectionByName(string connectionName, bool usePool = true);

        Connector GetConnection(bool usePool = true);

        Connector GetReadOnlyConnection(bool usePool = true);

        string GetConnectionString(string connectionName);

    }
}
