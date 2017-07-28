using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Adapter;
using ProfileService.IServiceInterfaces;
using ProfileService.Model;
using ProfileService.Model.BindingModel;
using ProfileService.Model.ViewModel;
using ProfileService.Services;
using RawRabbit;
using Vacation.common;
using Vacation.common.Events;

namespace ProfileService.Controllers
{
    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        private ITeamService _teamService;
        private  IEmployeeService _employeeService; 
        public TeamController(IEmployeeService employeeService, ITeamService teamService)
        {
            _teamService = teamService;
            _employeeService = employeeService; 
        }
        
        
        // GET api/values
        [HttpGet]
        [Route("GetAllTeam")]
        public IEnumerable<TeamViewModel> GetAllTeam()
        {
            IEnumerable<TeamViewModel> teamViewModel = TeamAdapter.ToTeamViewModel(_teamService.GetAllTeam());
            return teamViewModel;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody]TeamBindingModel teamBindingModel)
        {
            TeamAdapter._employeeService = _employeeService;
            if (_teamService.Create(TeamAdapter.ToTeamModel(teamBindingModel)) > 0)
            {
                return Ok(teamBindingModel);
            }
            else
            {
                return StatusCode(500);
            }
        }

        // PUT api/values/5
        [HttpPut]
        [Route("UpdateInfo")]
        public IActionResult Update([FromBody]TeamBindingModel teamBindingModel)
        {
            TeamAdapter._employeeService = _employeeService;
            if ( _teamService.Update(TeamAdapter.ToTeamModel(teamBindingModel)) > 0)
            {
                return Ok(teamBindingModel);
            }
            else
            {
                return BadRequest();
            }
            
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet]
        [Route("GetAllEmployees")]
        public IEnumerable<EmployeeTeamViewModel> GetAllEmployees()
        {
            return TeamAdapter.ToEmployeeViewModel(_employeeService.GetAll());
        }
    }
}
