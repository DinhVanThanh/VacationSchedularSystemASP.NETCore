using AuthenticationServer.Models.ViewModels;
using AuthenticationServer.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Adapters
{
    public static class PermissionsGroupAdapter
    {
        public static PermissionsGroupViewModel ToViewModel(this PermissionsGroup model)
        {
            if (model == null)
            {
                return null;
            }
            else
            {
                return new PermissionsGroupViewModel
                {
                    Name = model.Name,
                    Permissions = model.Permissions.ToListPermissionViewModels().ToArray()
                };
            }
        }

        public static IEnumerable<PermissionsGroupViewModel> ToListPermissionsGroupViewModels(this IEnumerable<PermissionsGroup> models)
        {
            if (models == null)
            {
                return null;
            }
            else
            {
                return models.Where(m => m != null).Select(m => m.ToViewModel());
            }
        }
    }
}
