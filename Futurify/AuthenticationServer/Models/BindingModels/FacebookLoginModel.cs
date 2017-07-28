using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models.BindingModels
{
    public class FacebookLoginModel
    {
        [Required]
        public string AccessToken { get; set; }
    }
    public class FacebookRegisterModel
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
    }
}
