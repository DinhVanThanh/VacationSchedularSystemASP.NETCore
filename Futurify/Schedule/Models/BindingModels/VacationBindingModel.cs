using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Models.BindingModels
{
    public class VacationBindingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; } 
        public int? OwnerId { get; set; }
        public Boolean IsAllDay { get; set; }
        public string Description { get; set; }
    }
}
