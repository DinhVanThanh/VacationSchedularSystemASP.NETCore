using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.common.core.Messaging;
using Microsoft.EntityFrameworkCore;
using ProfileService.Model;
using RawRabbit;
using RawRabbit.Context;
using Vacation.common.Events;


namespace ProfileService.EventHandlers
{
    public class RequestForgotPasswordForEmailHandler : IMessageHandle<RequestForgotPasswordForEmail>
    {
        private DbContextOptions<ProfileContext> _accountContext;

        private IBusClient _rawRabbitBus;

        public RequestForgotPasswordForEmailHandler(DbContextOptions<ProfileContext> accountContext, IBusClient rawRabbitBus)
        {
            _rawRabbitBus = rawRabbitBus;
            _accountContext = accountContext;
        }
        public async Task HandleAsync(RequestForgotPasswordForEmail e, IMessageContext context)
        {
            var db = new ProfileContext(_accountContext);
            var existingProfile = db.Employees.FirstOrDefault(t => t.Email == e.Email);
            if(existingProfile != null)
            {
                
            }
        }
    }
}
