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

namespace Notification.EventHandlers
{
    public class AccountCreatedHandlers : IMessageHandle<AccountCreatedForEmail>
    {
        private IHostingEnvironment _env;
        private IBusClient _busClient;
        private ConfigSendEmail _configSendMail;
        
        private DbContextOptions<Models.MessageContext> _context;

        public AccountCreatedHandlers(IBusClient busClient, IHostingEnvironment env, IOptions<ConfigSendEmail> configSendMail, DbContextOptions<Models.MessageContext> context)
        {
            _busClient = busClient;
            _env = env;
            _configSendMail = configSendMail.Value; 
            _context = context;
        }
        public async Task HandleAsync(AccountCreatedForEmail e, IMessageContext context)
        {
            //initiator new config for sending email
            ConfigSendEmail config = new ConfigSendEmail(_configSendMail);

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
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
