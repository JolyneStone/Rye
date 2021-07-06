using Dapper;

using Demo.Library.Abstraction;
using Demo.Library.Dto;

using Microsoft.Extensions.DependencyInjection;

using Rye.Enums;
using Rye.MySql;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Library.Service
{
    [Injection(ServiceLifetime.Scoped, InjectionPolicy.Replace, typeof(IPermissionService))]
    public class PermissionService: IPermissionService
    {
        private readonly MySqlConnectionProvider _connectionProvider;
        public PermissionService(MySqlConnectionProvider connectionProvider)
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
            IEnumerable<PermissionEntryDto<int>> list;
            using (var connector = _connectionProvider.GetReadOnlyConnection())
            {
                list = await connector.Connection.QueryAsync<PermissionEntryDto<int>>(sql, param: parameters);
            }

            if (list == null)
            {
                return Array.Empty<string>();
            }

            BulidCode(list);

            return list.Select(d => d.Code);

            void BulidCode(IEnumerable<PermissionEntryDto<int>> treeNodes)
            {
                int defaultVal = default;
                List<PermissionEntryDto<int>> trees = new List<PermissionEntryDto<int>>();

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
                                treeNode.Children = new List<PermissionEntryDto<int>>();
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
