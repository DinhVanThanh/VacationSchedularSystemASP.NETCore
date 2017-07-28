using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Vacation.common.Enums;

namespace Notification.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public int? ReceiverId { get; set; }
        public int? SenderId { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageType? Type { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? ReadAt { get; set; }
    }
}
