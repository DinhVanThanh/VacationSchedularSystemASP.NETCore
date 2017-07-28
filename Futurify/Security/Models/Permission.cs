using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Models
{
    public class Permission
    {
        public Permission() { }

        public Permission(string id, string display)
        {
            this.PermissionId = id;
            this.Display = display;
        }

        [Key]
        public string PermissionId { get; set; }
        public string Display { get; set; } 
        public virtual List<RolePermission> RolePermissions { get; set; }
    }
}
