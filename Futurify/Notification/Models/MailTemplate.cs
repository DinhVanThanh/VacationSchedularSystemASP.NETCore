using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.Models
{
    public class MailTemplate
    {
        public string Code { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
    public class ContainerMailTemplate
    {
        public MailTemplate[] MailTemplates;
    }
}
