using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Schedule.Models;
using Schedule.Models.BindingModels;
using Schedule.Models.ViewModels;


namespace Schedule.Adapters
{
    public static class VacationAdapter
    {
        public static Event ToModel( VacationBindingModel bindingModel)
        {
            Event vacation = new Event();
            vacation.EventId = bindingModel.Id;
            vacation.Start = bindingModel.Start;
            vacation.End = bindingModel.End;
            vacation.Description = bindingModel.Description;
            vacation.OwnerId = bindingModel.OwnerId;
            vacation.Title = bindingModel.Title;
            vacation.IsAllDay = bindingModel.IsAllDay;
            return vacation;
        }
        
        public static List<VacationViewModel> ToViewModel( IEnumerable<Event> Events)
        {
            List<VacationViewModel> viewModels;
            if (Events != null)
            {
                viewModels = new List<VacationViewModel>();
                foreach (Event e in Events)
                {
                    viewModels.Add(new VacationViewModel()
                    {
                        Id = e.EventId,
                        Title = e.Title,
                        Description = e.Description,
                        OwnerId = e.OwnerId,
                        IsAllDay = e.IsAllDay,
                        //Start = e.Start,
                        //End = e.End
                        Start = new DateTime(e.Start.Value.Year, e.Start.Value.Month, e.Start.Value.Day, e.Start.Value.Hour, e.Start.Value.Minute, e.Start.Value.Second, DateTimeKind.Utc),
                        End = new DateTime(e.End.Value.Year, e.End.Value.Month, e.End.Value.Day, e.End.Value.Hour, e.End.Value.Minute, e.End.Value.Second, DateTimeKind.Utc),
                    });
                }
                return viewModels;
            }
            else
            {
                return null;
            }            
        }
    }
}
