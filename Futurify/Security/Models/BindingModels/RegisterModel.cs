using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vacation.common.Enums;

namespace Security.Models.BindingModels
{
    public class RegisterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        [EmailAddress]
        public string Email { get; set; } 
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public int Position { get; set; }
        public DateTime Birthday { get; set; }

    }
}
