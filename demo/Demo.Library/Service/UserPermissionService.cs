using Demo.DataAccess.EFCore;
using Demo.DataAccess.EFCore.DbContexts;
using Demo.DataAccess.EFCore.Models;
using Demo.Library.Abstraction;
using Demo.Library.Dto;

using Rye.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Rye.Enums;
using System;

namespace Demo.Library.Service
{
    public class UserPermissionService : IUserPermissionService
    {
        private readonly IDbProvider _provider;

        public UserPermissionService(IDbProvider provider)
        {
            _provider = provider;
        }

        public async Task<IEnumerable<PermissionTreeDto>> GetPermissionsTreeByUserIdAsync(int userId)
        {
            var uow = _provider.GetUnitOfWorkByReadDb();
            var db = uow.DbContext.AsDbContext();

            var list = await (from userRole in db.Set<UserRole>()
                              join role in db.Set<Role>()
                              on userRole.RoleId equals role.Id
                              join rolePermission in db.Set<RolePermission>()
                              on userRole.RoleId equals rolePermission.RoleId
                              join permission in db.Set<Permission>()
                              on rolePermission.PermissionId equals permission.Id
                              where userRole.UserId == userId &&
                                    role.Status == (int)EntityStatus.Enabled &&
                                    permission.Status == (int)EntityStatus.Enabled
                              select new PermissionTreeDto
                              {
                                  Id = permission.Id,
                                  ParentId = permission.ParentId,
                                  Code = permission.Code,
                                  Name = permission.Name,
                                  Type = permission.Type,
                              }).ToListAsync();


            return BuildTree(list);
        }

        public async Task<IEnumerable<PermissionTreeDto>> GetPermissionsTreeByRolesIdAsync(int[] rolesId)
        {
            var uow = _provider.GetUnitOfWorkByReadDb();
            var db = uow.DbContext.AsDbContext();

            var list = await (from rolePermission in db.Set<RolePermission>()
                              join permission in db.Set<Permission>()
                              on rolePermission.PermissionId equals permission.Id
                              where rolesId.Contains(rolePermission.RoleId) &&
                                    permission.Status == (int)EntityStatus.Enabled
                              select new PermissionTreeDto
                              {
                                  Id = permission.Id,
                                  ParentId = permission.ParentId,
                                  Code = permission.Code,
                                  Name = permission.Name,
                                  Type = permission.Type,
                              }).ToListAsync();


            return BuildTree(list);
        }

        private static List<PermissionTreeDto> BuildTree(List<PermissionTreeDto> treeNodes)
        {
            List<PermissionTreeDto> trees = new List<PermissionTreeDto>();
            foreach (var treeNode in treeNodes)
            {
                if (treeNode.ParentId == 0)
                {
                    trees.Add(treeNode);
                }

                foreach (var it in treeNodes)
                {
                    if (it.ParentId == treeNode.Id)
                    {
                        if (treeNode.Children == null)
                        {
                            treeNode.Children = new List<PermissionTreeDto>();
                        }
                        it.Code = $"{treeNode.Code}.{it.Code}";
                        treeNode.Children.Add(it);
                    }
                }
            }

            return trees;
        }
    }
}
