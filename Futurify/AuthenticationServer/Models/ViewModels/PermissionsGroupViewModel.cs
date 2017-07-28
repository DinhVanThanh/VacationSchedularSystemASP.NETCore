using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models.ViewModels
{
    public class PermissionsGroupViewModel
    {
        public string Name { get; set; }
        public PermissionViewModel[] Permissions { get; set; }
    }
}
