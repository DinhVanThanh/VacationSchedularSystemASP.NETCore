using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models.BindingModels
{
    public class RoleBindingModel
    {
        [Required]
        public string Name { get; set; }
    }

    public class AssignRemovePermissionModel
    {
        [Required]
        public string PermissionId { get; set; }
    }

    public class AssignRemovePermissionsModel
    {
        [Required]
        public string[] PermissionsIds { get; set; }
    }
}
