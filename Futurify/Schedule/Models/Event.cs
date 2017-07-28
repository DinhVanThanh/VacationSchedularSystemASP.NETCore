using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using App.common.core.Models;

namespace Schedule.Models
{
    public class Event :BaseObject
    {
        [Key]
        public int EventId { get; set; }
        public string Title { get; set; }
        public int? OwnerId { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public bool IsAllDay { get; set; }
        public string Description { get; set; }
    }
}
