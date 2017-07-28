using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models.ViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool EmailVerified { get; set; }

        public bool PhoneNumberVerified { get; set; }

        public bool Locked { get; set; }

        public DateTime? LockedAt { get; set; }

        public DateTime? LockUntil { get; set; }

        public AccountViewModel LockBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public AccountViewModel CreatedBy { get; set; }

        public AccountViewModel ModifiedBy { get; set; }
    }
}
