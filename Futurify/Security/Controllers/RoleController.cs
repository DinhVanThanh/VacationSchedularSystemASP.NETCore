using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Security.IServiceInterfaces;

namespace Security.Controllers
{
    [Route("api/roles")]
    public class RoleController : Controller
    {
        private IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {

        }
    }
}
