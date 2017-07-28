using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models.BindingModels
{
    public class SetRemoveRoleModel
    {
        [Required]
        public int RoleId { get; set; }
    }
}
