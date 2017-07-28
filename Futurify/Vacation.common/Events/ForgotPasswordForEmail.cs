using System;
using System.Collections.Generic;
using System.Text;
using App.common.core.Messaging;

namespace Vacation.common.Events
{
    public class ForgotPasswordForEmail : IMessage
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        
    }
}
