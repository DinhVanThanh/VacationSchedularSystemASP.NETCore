using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.common.core.Messaging;
using App.common.core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Notification.IServiceInterfaces;
using Notification.Models;
using Notification.Services;
using RawRabbit;
using RawRabbit.Context;
using Vacation.common;
using Vacation.common.Events;
using App.common.core.Exceptions;
using System.Security.Claims;

namespace Notification.EventHandlers
{
    public class AccountCreatedHandlers : IMessageHandle<AccountCreatedForEmail>
    {
        private IHostingEnvironment _env;
        private IBusClient _busClient;
        private ConfigSendEmail _configSendMail;
        private MessageService _messageService;
        private DbContextOptions<Models.MessageContext> _context;
        private readonly ClaimsPrincipal _caller;
        public AccountCreatedHandlers(ClaimsPrincipal caller,IBusClient busClient, IHostingEnvironment env, IOptions<ConfigSendEmail> configSendMail, DbContextOptions<Models.MessageContext> context)
        {
            _caller = caller;
            _busClient = busClient;
            _env = env;
            _configSendMail = configSendMail.Value; 
            _context = context;
        }
        public async Task HandleAsync(AccountCreatedForEmail e, IMessageContext context)
        {
            //initiator new config for sending email
            ConfigSendEmail config = new ConfigSendEmail(_configSendMail);
            var User = _caller.Claims.Select(c => new { c.Type, c.Value });
            try
            {
                IMailService emailService = new MailService();
                IDataStaticService dataService = new DataStaticService(_env);
                MailTemplate mailTemplate = dataService.GetMailTemplate(CommonContants.AccountCreated);
                if (mailTemplate != null)
                {
                    config.Receivers = new List<string>() { e.Email };//send an email
                    await emailService.SendMail(config, mailTemplate, e);
                }
                Message message = new Message();
                message.ReceiverId = e.Id;
                message.Content = mailTemplate.Body;
                message.Subject = mailTemplate.Subject;
                message.Type = Vacation.common.Enums.MessageType.AccountCreated;
                message.CreateAt = DateTime.Now;
                _messageService.Create(message);
                
                
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, ex.InnerException.Message);
            }
        }
    }
}
