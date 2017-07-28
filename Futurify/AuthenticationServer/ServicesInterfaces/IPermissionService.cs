using AuthenticationServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.ServicesInterfaces
{
    public interface IPermissionService
    {
        Task<IList<Permission>> GetAllAsync();
        Task SyncPermissionsAsync(IEnumerable<Permission> permissions);
    }
}
