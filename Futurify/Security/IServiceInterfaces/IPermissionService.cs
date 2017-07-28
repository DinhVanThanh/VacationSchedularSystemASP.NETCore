using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Security.Models;

namespace Security.IServiceInterfaces
{
    public interface IPermissionService
    {
        Task<IList<Permission>> GetAllAsync();
        Task SyncPermissionsAsync(IEnumerable<Permission> permissions);
    }
}
