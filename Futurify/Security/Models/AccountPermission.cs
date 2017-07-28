using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Models
{
    public class AccountPermission
    {
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
        [ForeignKey("Permission")]
        public string PermissionId { get; set; }
        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }
    }
}
