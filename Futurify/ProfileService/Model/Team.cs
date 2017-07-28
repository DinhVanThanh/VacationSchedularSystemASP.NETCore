using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProfileService.Model
{
    public class Team 
    {
        [Key]
        public int TeamId { get; set; }
        [ForeignKey("Employee")]
        public int LeaderId { get; set; }

        [ForeignKey("LeaderId")]
        public Employee Leader { set; get; }

        public string TeamName { get; set; }

         [InverseProperty("Team")]
        public ICollection<Employee> Employees { get; set; }
    }
}
