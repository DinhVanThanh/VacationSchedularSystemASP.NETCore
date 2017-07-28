using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.common.core.Models;
using Notification.Models;

namespace Notification.IServiceInterfaces
{
    public interface IMailService
    {
        Task SendMail<T>(ConfigSendEmail config, MailTemplate mailTemplate, T value);
    }
}
