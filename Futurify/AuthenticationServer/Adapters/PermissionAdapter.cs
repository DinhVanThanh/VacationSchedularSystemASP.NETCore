using AuthenticationServer.Models;
using AuthenticationServer.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Adapters
{
    public static class PermissionAdapter
    {
        public static PermissionViewModel ToViewModel(this Permission model)
        {
            if (model == null)
            {
                return null;
            }
            else
            {
                return new PermissionViewModel
                {
                    Id = model.Id,
                    Display = model.Display
                };
            }
        }

        public static IEnumerable<PermissionViewModel> ToListPermissionViewModels(this IEnumerable<Permission> models)
        {
            if (models == null)
            {
                return new PermissionViewModel[] { };
            }
            else
            {
                return models.Where(m => m != null).Select(m => m.ToViewModel());
            }
        }
    }
}
