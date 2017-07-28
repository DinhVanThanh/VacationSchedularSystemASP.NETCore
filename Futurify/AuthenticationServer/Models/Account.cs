using JobHop.Common.Models;
using JobHop.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        
        public AccountType AccountType { get; set; }

        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string SecurityStamp { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }

        public UserStatus Status { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public bool EmailVerified { get; set; }

        public bool PhoneNumberVerified { get; set; }

        public bool Locked { get; set; }

        public bool IsSystemAdmin { get; set; }
        
        public DateTime? LockedAt { get; set; }

        public DateTime? LockUntil { get; set; }

        public virtual Account LockBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public virtual Account CreatedBy { get; set; }

        public virtual Account ModifiedBy { get; set; }

        public virtual List<VerificationCode> VerificationCodes { get; set; }

        public virtual List<ExternalProvider> ExternalLogins { get; set; }

        public virtual List<AccountRole> AccountRoles { get; set; }

        public virtual List<AccountPermission> AccountPermissions { get; set; }
    }

    public class ExternalProvider
    {
        [Required]
        public virtual Account Account { get; set; }
        public string Provider { get; set; }
        public string ProviderKey { get; set; }
    }

    public enum UserNameType { PhoneNumber, Email }
}
