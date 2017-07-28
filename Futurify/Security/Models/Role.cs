using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        public string Name { get; set; }

        public virtual List<RolePermission> RolePermissions { get; set; }

        public virtual List<AccountRole> AccountRoles { get; set; }
    }
}
