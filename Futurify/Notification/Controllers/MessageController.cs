using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.common.core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Notification.IServiceInterfaces;
using Notification.Models;
using Notification.Services;
using RawRabbit;
using RawRabbit.Context;
using Vacation.common;
using Vacation.common.Events;
using Vacation.common.Models;

namespace Notification.Controllers
{
    [Route("api/message")]
    public class MessageController : Controller
    {
        private IMailService _mailService;
        private IBusClient _rawRabbitBus;
        private ConfigSendEmail _configSendEmail;
        private IHostingEnvironment _env;
        public MessageController(IHostingEnvironment env, IBusClient rawRabbitBus, IMailService mailService, IOptions<ConfigSendEmail> configSendEmail)
        {
            _configSendEmail = configSendEmail.Value;
            _mailService = mailService;
            _rawRabbitBus = rawRabbitBus;
            _env = env;
        }
        // GET api/message/send
        [HttpPost]
        [Route("ForgotPassword/{Email}")]
        public async void SendEmailForPasswordRecovery(string Email)
        {
            ConfigSendEmail config = new ConfigSendEmail(_configSendEmail);

            try
            {
                PasswordRecovery PasswordRecoveryModel = new PasswordRecovery();
                PasswordRecoveryModel.ChangePasswordUrl = CommonContants.ForgotPasswordUrl;
                IMailService emailService = new MailService();
                IDataStaticService dataService = new DataStaticService(_env);
                MailTemplate mailTemplate = dataService.GetMailTemplate(CommonContants.ForgotPassword);
                if (mailTemplate != null)
                {
                    config.Receivers = new List<string>() { Email };//send an email
                    await emailService.SendMail(config, mailTemplate, PasswordRecoveryModel);
                }
            }
            catch (Exception e)
            {
                throw;
            } 
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
