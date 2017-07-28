using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Schedule.Models;
using Schedule.Models.BindingModels;

namespace Schedule.IServiceInterfaces
{
    public interface IVacationService
    {
        Event Get(int EventId);
        IEnumerable<Event> GetAllVacations();
        int Create(VacationBindingModel bindingModel);
        int Update(Event bindingModel);
        int Delete(int Id);
        IEnumerable<Event> GetEmployeeVacations(int Id);
    }
}
