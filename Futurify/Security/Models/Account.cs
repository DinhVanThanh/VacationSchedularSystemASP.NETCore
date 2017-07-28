using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Vacation.common.Enums;

namespace Security.Models
{
    public class Account
    {
        [Key]
         
        public int AccountId { get; set; } 
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public UserStatus? Status { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public AccountType? Type { get; set; }

        public bool? EmailVerified { get; set; }

        public bool? PhoneNumberVerified { get; set; }

        public bool? Locked { get; set; }

        public bool? IsSystemAdmin { get; set; }

        public DateTime? LockedAt { get; set; }

        public DateTime? LockUntil { get; set; }

        public virtual Account LockBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public virtual Account CreatedBy { get; set; }

        public virtual Account ModifiedBy { get; set; }

        public virtual List<VerificationCode> VerificationCodes { get; set; }
         
        public virtual List<AccountRole> AccountRoles { get; set; }

        public virtual List<AccountPermission> AccountPermissions { get; set; }
    }

     
}
