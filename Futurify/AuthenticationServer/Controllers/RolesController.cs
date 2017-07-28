using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthenticationServer.Models.ViewModels;
using AuthenticationServer.ServicesInterfaces;
using AuthenticationServer.Adapters;
using AuthenticationServer.Models.BindingModels;
using Microsoft.AspNetCore.Authorization;
using App.Common.Core.Exceptions;
using AuthenticationServer.Resources;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthenticationServer.Controllers
{
    [Route("api/roles")]
    public class RolesController : Controller
    {
        IRoleService _roleService;
        IPermissionService _permissionService;

        public RolesController(IRoleService roleService, IPermissionService permissionService)
        {
            _roleService = roleService;
            _permissionService = permissionService;
        }

        // GET: api/roles
        [Authorize(Roles = "VIEW_ROLES")]
        [HttpGet]
        public async Task<IEnumerable<RoleViewModel>> GetAllRoles()
        {
            var roles = await _roleService.GetAllAsync();

            return roles.ToListRoleViewModels();
        }

        // GET api/roles/{id}
        [Authorize(Roles = "VIEW_ROLES")]
        [HttpGet("{id}")]
        public async Task<RoleViewModel> GetRole(int id)
        {
            var role = await _roleService.GetAsync(id);

            return role.ToViewModel();
        }

        // POST api/roles
        [Authorize(Roles = "CREATE_ROLE")]
        [HttpPost]
        public async Task<RoleViewModel> CreateRole([FromBody]RoleBindingModel bindingModel)
        {
            if (bindingModel == null || !ModelState.IsValid)
            {
                throw new CustomException(Errors.INVALID_ROLE_BINDING_DATA, Errors.INVALID_ROLE_BINDING_DATA_MSG);
            }

            var role = bindingModel.ToModel();

            role = await _roleService.CreateAsync(role);

            return role.ToViewModel();
        }

        // PUT api/roles/{id}
        [Authorize(Roles = "EDIT_ROLE")]
        [HttpPut("{id}")]
        public async Task<RoleViewModel> UpdateRole(int id, [FromBody]RoleBindingModel bindingModel)
        {
            if (bindingModel == null || !ModelState.IsValid)
            {
                throw new CustomException(Errors.INVALID_ROLE_BINDING_DATA, Errors.INVALID_ROLE_BINDING_DATA_MSG);
            }

            var role = bindingModel.ToModel(id);

            role = await _roleService.UpdateAsync(role);

            return role.ToViewModel();
        }

        // DELETE api/roles/{id}
        [Authorize(Roles = "DELETE_ROLE")]
        [HttpDelete("{id}")]
        public async Task DeleteRole(int id)
        {
            await _roleService.DeleteAsync(id);
        }

        // PUT api/roles/{roleId}/assign-permission
        [Authorize(Roles = "EDIT_ROLE")]
        [HttpPut, Route("{roleId}/assign-permission")]
        public async Task AssignPermission(int roleId, [FromBody]AssignRemovePermissionModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                throw new CustomException(Errors.INVALID_ROLE_PERMISSION_COMMAND_DATA, Errors.INVALID_ROLE_PERMISSION_COMMAND_DATA_MSG);
            }

            await _roleService.AssignPermissionAsync(roleId, model.PermissionId);
        }

        // PUT api/roles/{roleId}/assign-permissions
        [Authorize(Roles = "EDIT_ROLE")]
        [HttpPut, Route("{roleId}/assign-permissions")]
        public async Task AssignPermissions(int roleId, [FromBody]AssignRemovePermissionsModel model)
        {
            if (model == null || !ModelState.IsValid || !model.PermissionsIds.Any())
            {
                throw new CustomException(Errors.INVALID_ROLE_PERMISSION_COMMAND_DATA, Errors.INVALID_ROLE_PERMISSION_COMMAND_DATA_MSG);
            }

            await _roleService.AssignPermissionsAsync(roleId, model.PermissionsIds);
        }

        // PUT api/roles/{roleId}/assign-permissions
        [Authorize(Roles = "EDIT_ROLE")]
        [HttpPut, Route("{roleId}/remove-permission")]
        public async Task RemovePermission(int roleId, [FromBody]AssignRemovePermissionModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                throw new CustomException(Errors.INVALID_ROLE_PERMISSION_COMMAND_DATA, Errors.INVALID_ROLE_PERMISSION_COMMAND_DATA_MSG);
            }

            await _roleService.RemovePermissionAsync(roleId, model.PermissionId);
        }

        // PUT api/roles/{roleId}/assign-permissions
        [Authorize(Roles = "EDIT_ROLE")]
        [HttpPut, Route("{roleId}/remove-permissions")]
        public async Task RemovePermissions(int roleId, [FromBody]AssignRemovePermissionsModel model)
        {
            if (model == null || !ModelState.IsValid || !model.PermissionsIds.Any())
            {
                throw new CustomException(Errors.INVALID_ROLE_PERMISSION_COMMAND_DATA, Errors.INVALID_ROLE_PERMISSION_COMMAND_DATA_MSG);
            }

            await _roleService.RemovePermissionsAsync(roleId, model.PermissionsIds);
        }

        // GET api/roles/{roleId}/permissions
        [Authorize(Roles = "VIEW_ROLES")]
        [HttpGet, Route("{roleId}/permissions")]
        public async Task<IEnumerable<PermissionViewModel>> GetPermissionsOfRole(int roleId)
        {
            var permissions = await _roleService.GetPermissionsOfRoleAsync(roleId);
            return permissions.ToListPermissionViewModels();
        }

        // GET api/roles/permissions
        [Authorize(Roles = "VIEW_ROLES")]
        [HttpGet, Route("permissions")]
        public async Task<IEnumerable<RolePermissionsViewModel>> GetPermissionsOfRoles()
        {
            var roles = await _roleService.GetAllRolesPermissions();

            return roles.ToListRolePermissionsViewModels();
        }
    }
}
