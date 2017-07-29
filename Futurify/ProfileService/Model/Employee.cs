using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Vacation.common.Enums;

namespace ProfileService.Model
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Gender? Gender { get; set; }

        public string Email { get; set; }

        [ForeignKey("Position")]
        public int? PositionId { get; set; }
        
        [ForeignKey("PositionId")]
        public Position Position { get; set; }

        public string Avatar { get; set; } 

        [ForeignKey("Team")]
        public int? TeamId { get; set; }

        [ForeignKey("TeamId")]
        public Team Team { get; set; }

        public int? RemainingDay { get; set; }

        public int? AccountId { get; set; }
    }
}
