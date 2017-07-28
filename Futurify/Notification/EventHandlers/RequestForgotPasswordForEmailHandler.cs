using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.common.core.Exceptions;
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
    public class RequestForgotPasswordForEmailHandler : IMessageHandle<RequestForgotPasswordForEmail>
    {
        private IHostingEnvironment _env;
        private IBusClient _busClient;
        private ConfigSendEmail _configSendMail;
        private DbContextOptions<Models.MessageContext> _messContext;

        public RequestForgotPasswordForEmailHandler(IBusClient busClient, IHostingEnvironment env, IOptions<ConfigSendEmail> configSendMail, DbContextOptions<Models.MessageContext> messContext)
        {
            _busClient = busClient;
            _env = env;
            _configSendMail = configSendMail.Value;
            _messContext = messContext;
        }

        public async Task HandleAsync(RequestForgotPasswordForEmail e, IMessageContext context)
        {
            ConfigSendEmail config = new ConfigSendEmail(_configSendMail);
            
            try
            {
                IMailService emailService = new MailService();
                IDataStaticService dataService = new DataStaticService(_env);
                MailTemplate mailTemplate = dataService.GetMailTemplate(CommonContants.ForgotPassword);
                if (mailTemplate != null)
                {
                    config.Receivers = new List<string>() { e.Email };//send an email
                    await emailService.SendMail(config, mailTemplate, e);
                }
            }
            catch (Exception ex)
            {
                throw new CustomException("CAN_NOT_SEND_EMAIL", ex.Message);
            }
        }
    }
}
