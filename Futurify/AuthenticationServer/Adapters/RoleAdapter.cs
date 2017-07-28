using AuthenticationServer.Models;
using AuthenticationServer.Models.BindingModels;
using AuthenticationServer.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Adapters
{
    public static class RoleAdapter
    {
        public static Role ToModel(this RoleBindingModel bindingModel, int? roleId = null)
        {
            if (bindingModel == null)
            {
                return null;
            }
            else
            {
                var model = new Role
                {
                    Name = bindingModel.Name
                };

                if (roleId.HasValue)
                {
                    model.Id = roleId.Value;
                }

                return model;
            }
        }

        public static RoleViewModel ToViewModel(this Role model)
        {
            if (model == null)
            {
                return null;
            }
            else
            {
                var viewModel = new RoleViewModel
                {
                    Id = model.Id,
                    Name = model.Name
                };

                return viewModel;
            }
        }

        public static IEnumerable<RoleViewModel> ToListRoleViewModels(this IEnumerable<Role> models)
        {
            if (models == null)
            {
                return new RoleViewModel[] { };
            }
            else
            {
                return models.Where(m => m != null).Select(m => m.ToViewModel());
            }
        }

        public static RolePermissionsViewModel ToRolePermissionsViewModel(this Role model)
        {
            if (model == null)
            {
                return null;
            }
            else
            {
                return new RolePermissionsViewModel
                {
                    Id = model.Id,
                    Name = model.Name,
                    Permissions = model.RolePermissions.Select(r => r.Permission).ToListPermissionViewModels()
                };
            }
        }

        public static IEnumerable<RolePermissionsViewModel> ToListRolePermissionsViewModels(this IEnumerable<Role> models)
        {
            if (models == null)
            {
                return new RolePermissionsViewModel[] { };
            }
            else
            {
                return models.Where(r => r != null).Select(r => r.ToRolePermissionsViewModel());
            }
        }
    }
}
