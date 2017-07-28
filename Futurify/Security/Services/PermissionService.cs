using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Security.IServiceInterfaces;
using Security.Models;

namespace Security.Services
{
    public class PermissionService : IPermissionService
    {
        private AuthContext _context;

        public PermissionService(AuthContext context)
        {
            _context = context;
        }

        public async Task<IList<Permission>> GetAllAsync()
        {
            return await _context.Permissions.ToListAsync();
        }

        public async Task SyncPermissionsAsync(IEnumerable<Permission> permissions)
        {
            var allPermissions = await this.GetAllAsync();
            var syncCodes = permissions.Select(p => p.PermissionId).ToList();
            if (allPermissions != null)
            {
                var updatePermissions = allPermissions.Where(p => syncCodes.Contains(p.PermissionId));
                var deletePermissions = allPermissions.Except(updatePermissions).ToList();
                var addPermissions = permissions.Where(p => !updatePermissions.Any(u => u.PermissionId == p.PermissionId)).ToList();

                foreach (var permission in updatePermissions)
                {
                    permission.Display = permissions.First(p => p.PermissionId == permission.PermissionId).Display;
                }

                if (deletePermissions.Any())
                {
                    _context.RolePermissions.RemoveRange(_context.RolePermissions.Where(r => deletePermissions.Any(d => d.PermissionId == r.PermissionId)));
                    _context.Permissions.RemoveRange(deletePermissions);
                }

                if (addPermissions.Any())
                {
                    _context.Permissions.AddRange(addPermissions);
                }

                await _context.SaveChangesAsync();
            } 
        }
    }
}
