using Dapper;

using Rye.Entities.Abstractions;
using Rye.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rye.SqlServer.Service
{
    public class SqlServerPermissionService : IPermissionService
    {
        private readonly SqlServerConnectionProvider _connectionProvider;
        public SqlServerPermissionService(SqlServerConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public virtual async Task<IEnumerable<string>> GetPermissionCodeAsync(string roleIds)
        {
            var sql = @"select distinct p.code from permission AS p WITH(NOLOCK)
                        join rolePermission AS rp WITH(NOLOCK) on p.id = rp.permissionId
                        where p.status = @status and rp.roleId in @roleIds";
            var parameters = new DynamicParameters();
            parameters.Add("@status", EntityStatus.Enabled);
            parameters.Add("@roleIds", roleIds.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
            using (var conn = _connectionProvider.GetReadOnlyConnection())
            {
                return await conn.QueryAsync<string>(sql, param: parameters);
            }
        }
    }
}
