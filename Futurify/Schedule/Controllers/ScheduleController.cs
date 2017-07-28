using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Schedule.Adapters;
using Schedule.IServiceInterfaces;
using Schedule.Models;
using Schedule.Models.BindingModels;
using Schedule.Models.ViewModels;

namespace Schedule.Controllers
{
    [Route("api/schedule")]
    public class ScheduleController : Controller
    {
        private IVacationService _vacationService;
        public ScheduleController(IVacationService vacationService)
        {
            _vacationService = vacationService;
        }
        // GET api/schedule/GetAllVacations
        [HttpGet]
        [Route("GetAllVacations/{Id}")]
        public IEnumerable<VacationViewModel> GetAllVacation(int? Id)
        {
            if(Id.HasValue && Id == -1)
            {
                var Vacations = _vacationService.GetAllVacations();
                if (Vacations != null)
                {
                    return VacationAdapter.ToViewModel(Vacations);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return VacationAdapter.ToViewModel(_vacationService.GetEmployeeVacations((int)Id));
            }

            //return Ok(new List<VacationViewModel> { new VacationViewModel() { Id = 3, Start = new DateTime(2017,7,19, 7, 30, 0, DateTimeKind.Utc), End = new DateTime(2017, 7, 19, 12, 30, 0, DateTimeKind.Utc),
            //OwnerId = 1, IsAllDay = false, Title= "asdadfdsfasd", Description = "ok hello ae"} });

        }
        // POST api/schedule/Create
        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody]VacationBindingModel vacationBindingModel)
        {
            if (vacationBindingModel != null)
            {
                vacationBindingModel.Id = _vacationService.Create(vacationBindingModel);
                if (vacationBindingModel.Id != 0)
                {
                    return Ok(vacationBindingModel);
                }
                else
                    return BadRequest(vacationBindingModel);
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT api/schedule/Update
        [HttpPut("Update")]
        public IActionResult Update([FromBody]VacationBindingModel vacationBindingModel)
        {
            if(vacationBindingModel != null)
            {
                if (_vacationService.Update(VacationAdapter.ToModel(vacationBindingModel)) != 0)
                    return Ok(vacationBindingModel);
                else
                    return BadRequest(vacationBindingModel);
            }
            else
            {
                return BadRequest();
            }
            
            
        }

        // DELETE api/schedule/Delete/{id}
        [HttpDelete("Delete/{Id}")]
        public IActionResult Delete(int? Id)
        {
            if (Id.HasValue)
            {
                if (_vacationService.Delete((int)Id) != 0)
                    return Ok();
                else
                    return BadRequest();
            } 
            else
                return BadRequest();
        }
    }
}
