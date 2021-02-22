using System;
using System.Data;

namespace Rye.DataAccess
{
    public static class ConnectionProviderExtension
    {
        public static string GetConnectionString(this IConnectionProvider provider, Enum @enum)
        {
            Check.NotNull(provider, nameof(provider));
            Check.NotNull(@enum, nameof(@enum));
            return provider.GetConnectionString(@enum.GetDescription());
        }

        public static IDbConnection GetDbConnectionByName(this IConnectionProvider provider, Enum @enum)
        {
            Check.NotNull(provider, nameof(provider));
            Check.NotNull(@enum, nameof(@enum));
            return provider.GetDbConnectionByName(@enum.GetDescription());
        }
    }
}
