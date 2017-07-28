using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using App.common.core.Messaging;
using Vacation.common.Enums;

namespace Vacation.common.Events
{
    public class AccountCreatedForEmail : IMessage
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public int Position { get; set; }
        public DateTime Birthday { get; set; }
        public string Password { get; set; }
        [Url]
        public string LoginUrl { get; set; }
    }
}
