using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using App.common.core.Messaging;

namespace Vacation.common.Events
{
    public class RequestForgotPasswordForEmail : IMessage
    {
        public string Email { get; set; }  
        [Url]
        public string ChangePasswordUrl { get; set; }
        public string Code { get; set; }
    }
}
