using Dapper;

using Rye.Entities.Abstractions;
using Rye.Entities.Internal;
using Rye.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rye.MySql.Service
{
    public class MySqlPermissionService<TPermissionKey> : IPermissionService<TPermissionKey>
        where TPermissionKey : IEquatable<TPermissionKey>
    {
        private readonly MySqlConnectionProvider _connectionProvider;
        public MySqlPermissionService(MySqlConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public virtual async Task<IEnumerable<string>> GetPermissionCodeAsync(string roleIds)
        {
            var sql = @"select distinct p.`id`, p.`parentId`, p.`code` from `permission` AS p
                        join `rolePermission` AS rp on p.id = rp.permissionId
                        where p.status = @status and rp.roleId in @roleIds";
            var parameters = new DynamicParameters();
            parameters.Add("@status", EntityStatus.Enabled);
            parameters.Add("@roleIds", roleIds.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
            IEnumerable<PermissionEntry<TPermissionKey>> list;
            using (var connector = _connectionProvider.GetReadOnlyConnection())
            {
                list = await connector.Connection.QueryAsync<PermissionEntry<TPermissionKey>>(sql, param: parameters);
            }

            if (list == null)
            {
                return Array.Empty<string>();
            }

            BulidCode(list);

            return list.Select(d => d.Code);

            void BulidCode(IEnumerable<PermissionEntry<TPermissionKey>> treeNodes)
            {
                var defaultVal = default(TPermissionKey);
                List<PermissionEntry<TPermissionKey>> trees = new List<PermissionEntry<TPermissionKey>>();

                foreach (var treeNode in treeNodes)
                {
                    if (defaultVal.Equals(treeNode.ParentId))
                    {
                        trees.Add(treeNode);
                    }

                    foreach (var it in treeNodes)
                    {
                        if (it.ParentId.Equals(treeNode.Id))
                        {
                            if (treeNode.Children == null)
                            {
                                treeNode.Children = new List<PermissionEntry<TPermissionKey>>();
                            }
                            it.Code = $"{treeNode.Code}.{it.Code}";
                            treeNode.Children.Add(it);
                        }
                    }
                }
            }
        }
    }
}
