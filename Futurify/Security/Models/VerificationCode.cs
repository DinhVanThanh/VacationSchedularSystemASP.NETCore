using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Security.Models
{
    public class VerificationCode
    {
        [Key]
        public int Id { get; set; }  
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public VerificationReceiverType ReceiverType { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public VerificationPurpose Purpose { get; set; }
        public string Code { get; set; } 
        public bool Used { get; set; }
        public int SendCounter { get; set; }
        public int CheckFailedCounter { get; set; }
        public string CodeReceiver { get; set; }
    }
    public enum VerificationReceiverType
    {
        Email
    }
    public enum VerificationPurpose
    {
        RegistrationPhoneNumber,
        ResetPassword
    }
}
