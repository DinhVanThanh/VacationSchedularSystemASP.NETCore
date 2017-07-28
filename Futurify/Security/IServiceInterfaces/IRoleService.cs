using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Security.Models;

namespace Security.IServiceInterfaces
{
    public interface IRoleService
    {
        Task<Role> CreateAsync(Role role);
        Task<Role> UpdateAsync(Role role);
        Task DeleteAsync(int id);
        Task<IList<Role>> GetAllAsync();
        Task<IList<Role>> GetAllRolesPermissions();
        Task<IEnumerable<Permission>> GetPermissionsOfRoleAsync(int roleId);
        Task<Role> GetAsync(int id);
        Task<bool> IsDuplicatedRoleNameAsync(string name, int? id);
        Task AssignPermissionAsync(int roleId, string permissionCode);
        Task AssignPermissionsAsync(int roleId, string[] permissionCodes);
        Task RemovePermissionAsync(int roleId, string permissionCode);
        Task RemovePermissionsAsync(int roleId, string[] permissionCodes);
    }
}
