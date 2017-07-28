using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vacation.common.Models
{
    public class PasswordRecovery
    { 
        [Url]
        public string ChangePasswordUrl { get; set; }
    }
}
