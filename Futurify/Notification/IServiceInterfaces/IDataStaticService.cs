using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Notification.Models;

namespace Notification.IServiceInterfaces
{
    public interface IDataStaticService
    {
        MailTemplate GetMailTemplate(string code);
    }
}
