using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Schedule.Adapters;
using Schedule.IServiceInterfaces;
using Schedule.Models;
using Schedule.Models.BindingModels;

namespace Schedule.Services
{
    public class VacationService : IVacationService
    {
        private ScheduleContext _context;
        public VacationService(ScheduleContext context)
        {
            _context = context;
        }
        public int Create(VacationBindingModel bindingModel)
        {
            _context.Vacations.Add(VacationAdapter.ToModel(bindingModel));
            _context.SaveChanges();
            Event createdEvent = _context.Vacations.OrderByDescending(t => t.EventId).Take(1).FirstOrDefault();
            return createdEvent != null ? createdEvent.EventId : 0;
        }

        public int Delete(int Id)
        {
            Event existingVacation = Get(Id);
            _context.Vacations.Remove(existingVacation);
            return _context.SaveChanges();
        }

        public Event Get(int EventId)
        {
            var vacation = _context.Vacations.Find(EventId);
            return vacation != null ? vacation : null ;
        }

        public IEnumerable<Event> GetAllVacations()
        {
            return _context.Vacations.ToList();
        }

        public IEnumerable<Event> GetEmployeeVacations(int Id)
        {
            return _context.Vacations.Where(v => v.OwnerId == Id).ToList();
        }

        public int Update(Event model)
        {
            Event existingVacation = Get(model.EventId);

            _context.Vacations.Update(existingVacation);
            return _context.SaveChanges();
        }
    }
}
