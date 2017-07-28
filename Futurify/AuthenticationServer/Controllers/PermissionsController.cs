using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthenticationServer.Setup;
using AuthenticationServer.Models;
using AuthenticationServer.Models.ViewModels;
using AuthenticationServer.Adapters;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthenticationServer.Controllers
{
    [Route("api/permissions")]
    public class PermissionsController : Controller
    {
        // GET: api/permissions
        [HttpGet]
        public IEnumerable<PermissionViewModel> GetAllPermissions()
        {
            return PermissionsList.GroupsPermissions.SelectMany(p => p.Permissions).ToListPermissionViewModels();
        }

        // GET: api/permissions/groups
        [HttpGet("groups")]
        public IEnumerable<PermissionsGroupViewModel> GetGroupsPermissions()
        {
            return PermissionsList.GroupsPermissions.ToListPermissionsGroupViewModels();
        }
    }
}
