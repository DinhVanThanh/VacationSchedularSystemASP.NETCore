using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Notification.Models;
using Notification.Models.BiindingModel;
using Notification.Models.ViewModels;

namespace Notification.Adapters
{
    public static class MessageAdapter
    {
        public static MessageViewModel ToViewModel(Message message)
        {
            MessageViewModel viewModel = new MessageViewModel();
            viewModel.Id = message.Id;
            viewModel.Receiver = message.ReceiverId;
            viewModel.Sender = message.SenderId;
            viewModel.Subject = message.Subject;
            viewModel.Content = message.Content;
            viewModel.Type = message.Type;
            viewModel.IsRead = message.ReadAt != null ? true : false;
            viewModel.CreateAt = message.CreateAt;

            return viewModel;
        }
        public static IEnumerable<MessageViewModel> ToViewModel(IList<Message> messages)
        {
            IList<MessageViewModel> viewModels = new List<MessageViewModel>();
            foreach(Message message in messages)
            {
                viewModels.Add(ToViewModel(message));
            }
            return viewModels;
        }
        public static Message ToModel(MessageBindingModel messageBindingModel)
        {
            Message message = new Message();
            message.ReceiverId = messageBindingModel.ReceiverId;
            message.SenderId = messageBindingModel.SenderId;
            message.Subject = messageBindingModel.Subject;
            message.Content = messageBindingModel.Content;
            message.CreateAt = DateTime.Now;
            message.Type = messageBindingModel.Type;

            return message;
        }
    }
}
