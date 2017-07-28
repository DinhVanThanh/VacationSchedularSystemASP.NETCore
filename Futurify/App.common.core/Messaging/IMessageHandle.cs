using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RawRabbit.Context;

namespace App.common.core.Messaging
{
    public interface IMessageHandle<TMessage> where TMessage : IMessage
    {
        Task HandleAsync(TMessage e, IMessageContext context);
    }
    public delegate Task HandleAsync<in TMessage>(TMessage e, IMessageContext context) where TMessage : IMessage;
}
