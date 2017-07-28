using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Vacation.common.Enums;

namespace Notification.Models.ViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public int? Sender { get; set; }
        public int? Receiver { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageType? Type { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? CreateAt { get; set; }

    }
}
