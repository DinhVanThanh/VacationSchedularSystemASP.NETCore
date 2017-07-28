using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Models
{
    public class RolePermission
    {
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        [ForeignKey("Permission")]
        public string PermissionId { get; set; }
        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }
    }
}
