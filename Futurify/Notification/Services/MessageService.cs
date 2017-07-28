using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.common.core.Exceptions;
using Notification.Adapters;
using Notification.IServiceInterfaces;
using Notification.Models;
using Notification.Models.BiindingModel;
using Notification.Models.ViewModels;

namespace Notification.Services
{
    public class MessageService : IMessageService
    {
        private MessageContext _context;
        public MessageService(MessageContext context)
        {
            _context = context;
        }

        public int Create(Message message)
        {
            _context.Messages.Add(message);
            return _context.SaveChanges();
        }

        public IEnumerable<MessageViewModel> GetListMessage()
        {
            return MessageAdapter.ToViewModel(_context.Messages.ToList());
        }

        public Message SendMessage(MessageBindingModel message)
        {
            if (!message.SenderId.HasValue || !message.ReceiverId.HasValue || message.Content == null)
                throw new CustomException("Errors.INVALID_INPUT", "Errors.INVALID_INPUT");
            Message _message = MessageAdapter.ToModel(message);
            Create(_message); 
            return _message;
        }
    }
}
