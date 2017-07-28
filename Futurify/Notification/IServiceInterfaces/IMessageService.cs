using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Notification.Models;
using Notification.Models.BiindingModel;
using Notification.Models.ViewModels;

namespace Notification.IServiceInterfaces
{
    public interface IMessageService
    {
         Message SendMessage(MessageBindingModel message);

        IEnumerable<MessageViewModel> GetListMessage();
        int Create(Message message);
    }
}
