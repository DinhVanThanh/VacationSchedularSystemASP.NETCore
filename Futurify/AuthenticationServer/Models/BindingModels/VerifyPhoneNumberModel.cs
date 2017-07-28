using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models.BindingModels
{
    public class VerifyPhoneNumberModel
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string PIN { get; set; }
    }
}
