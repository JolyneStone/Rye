using Microsoft.Extensions.ObjectPool;

namespace Rye.DataAccess.Pool
{
    public class ConnectionPoolPolicy : PooledObjectPolicy<Connector>
    {
        private readonly ConnectionProvider _connectionProvider;
        private readonly string _connectionString;

        public ConnectionPoolPolicy(ConnectionProvider connectionProvider, string connectionString)
        {
            _connectionProvider = connectionProvider;
            _connectionString = connectionString;
        }
        public override Connector Create()
        {
            return _connectionProvider.GetDbConnection(_connectionString, false);
        }

        public override bool Return(Connector obj)
        {
            obj?.Return();
            return true;
        }
    }
}
