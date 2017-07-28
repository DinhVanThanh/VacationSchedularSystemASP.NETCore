using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vacation.common.Enums;

namespace Notification.Models.BiindingModel
{
    public class MessageBindingModel
    {
        public int? ReceiverId { get; set; }
        public int? SenderId { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public MessageType? Type
        {
            get; set;
        }
    }
}
