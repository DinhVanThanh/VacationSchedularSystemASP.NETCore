using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Model.ViewModel
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public  string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Gender { get; set; }
        public int PositionId { get; set; }  
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int RemainDayOff { get; set; }
        public string Avatar { get; set; }
    }
}
