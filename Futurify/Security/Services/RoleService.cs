using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.common.core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Security.IServiceInterfaces;
using Security.Models;

namespace Security.Services
{
    public class RoleService  : IRoleService
    {
        private AuthContext _context;

        public RoleService(AuthContext context)
        {
            _context = context;
        }

        public async Task AssignPermissionAsync(int roleId, string permissionId)
        {
            var existRole = _context.Roles.FirstOrDefault(r => r.RoleId == roleId);

            if (existRole == null)
            {
                throw new CustomException("Errors.ROLE_NOT_FOUND");
            }

            var existPermission = _context.Permissions.FirstOrDefault(p => p.PermissionId.ToLower() == permissionId.ToLower());

            if (existPermission == null)
            {
                throw new CustomException("Errors.PERMISSION_NOT_FOUND");
            }

            if (!_context.RolePermissions.Any(r => r.RoleId == roleId && r.PermissionId == permissionId))
            {

                _context.RolePermissions.Add(new RolePermission
                {
                    Permission = existPermission,
                    Role = existRole
                });

                await _context.SaveChangesAsync();
            }
        }

        public async Task AssignPermissionsAsync(int roleId, string[] permissionCodes)
        {
            var existRole = _context.Roles.FirstOrDefault(r => r.RoleId == roleId);

            if (existRole == null)
            {
                throw new CustomException("Errors.ROLE_NOT_FOUND");
            }

            var addedPermissions = _context.RolePermissions.Where(r => r.RoleId == roleId && permissionCodes.Contains(r.PermissionId)).Select(r => r.PermissionId);

            var addPermissions = permissionCodes.Except(addedPermissions).ToArray();

            var existPermissions = _context.Permissions.Where(p => addPermissions.Contains(p.PermissionId)).ToList();

            if (existPermissions.Count != addPermissions.Length)
            {
                throw new CustomException("Errors.PERMISSION_NOT_FOUND");
            }

            if (existPermissions.Any())
            {
                _context.RolePermissions.AddRange(existPermissions.Select(r => new RolePermission
                {
                    Permission = r,
                    Role = existRole
                }));

                await _context.SaveChangesAsync();
            }
        }

        public async Task<Role> CreateAsync(Role role)
        {
            if (await this.IsDuplicatedRoleNameAsync(role.Name, null))
            {
                throw new CustomException("Errors.ROLE_DUPLICATED_NAME", "Errors.ROLE_DUPLICATED_NAME_MSG");
            }

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return role;
        }

        public async Task<IList<Role>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetAsync(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == id);
        }

        public async Task RemovePermissionAsync(int roleId, string permissionId)
        {
            var rolePermission = await _context.RolePermissions.FirstOrDefaultAsync(r => r.RoleId == roleId && r.PermissionId == permissionId);

            if (rolePermission != null)
            {
                _context.RolePermissions.Remove(rolePermission);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemovePermissionsAsync(int roleId, string[] permissionsIds)
        {
            var rolesPermissions = await _context.RolePermissions.Where(r => r.RoleId == roleId && permissionsIds.Contains(r.PermissionId)).ToListAsync();

            if (rolesPermissions.Any())
            {
                _context.RolePermissions.RemoveRange(rolesPermissions);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<Role> UpdateAsync(Role role)
        {
            var existRole = _context.Roles.FirstOrDefault(r => role.RoleId == r.RoleId);
            if (existRole == null)
            {
                throw new CustomException("Errors.ROLE_NOT_FOUND", "Errors.ROLE_NOT_FOUND_MSG");
            }

            if (await IsDuplicatedRoleNameAsync(role.Name, role.RoleId))
            {
                throw new CustomException("Errors.ROLE_DUPLICATED_NAME", "Errors.ROLE_DUPLICATED_NAME_MSG");
            }

            existRole.Name = role.Name;
            await _context.SaveChangesAsync();

            return existRole;
        }

        public async Task<bool> IsDuplicatedRoleNameAsync(string name, int? id)
        {
            if (id.HasValue)
            {
                return await _context.Roles.AnyAsync(t => t.RoleId != id.Value && t.Name.ToLower() == name.ToLower());
            }
            else
            {
                return await _context.Roles.AnyAsync(t => t.Name.ToLower() == name.ToLower());
            }
        }

        public async Task DeleteAsync(int id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == id);

            if (role == null)
            {
                throw new CustomException("Errors.ROLE_NOT_FOUND", "Errors.ROLE_NOT_FOUND_MSG");
            }

            _context.AccountsRoles.RemoveRange(_context.AccountsRoles.Where(a => a.RoleId == id));
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Permission>> GetPermissionsOfRoleAsync(int roleId)
        {
            return await _context.RolePermissions.Where(r => r.RoleId == roleId).Select(r => r.Permission).ToListAsync();
        }

        public async Task<IList<Role>> GetAllRolesPermissions()
        {
            return await _context.Roles.Include(r => r.RolePermissions).ThenInclude(r => r.Permission).ToListAsync();
        }
    }
}
