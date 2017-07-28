using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.common.core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Notification.IServiceInterfaces;
using Notification.Models;
using Notification.Services; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Notification.EventHandlers;
using Vacation.common.Events;
using RawRabbit.vNext;
using RawRabbit;
using App.common.core;

namespace Notification
{
    public class Startup
    {
        private string _contentRootPath;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            _contentRootPath = env.ContentRootPath;
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MessageContext>(options => options.UseSqlServer(Configuration.GetConnectionString("NotificationDatabase")));
            services.Configure<ConfigSendEmail>(Configuration.GetSection("ConfigSendEmail"));
            services.AddRawRabbit(cfg => cfg.SetBasePath(_contentRootPath).AddJsonFile("rabbitmq.json"));
            services.AddScoped<IMessageService, MessageService>();
            services.AddSingleton<IDataStaticService, DataStaticService>();
            services.AddScoped<IMailService, MailService>();
            services.Configure<MicroserviceSetting>(Configuration.GetSection("Microservices"));
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            MessageContext.UpdateDatabase(app);
            app.UseMvc(); 
            _setupEvents(app, env);
        }

        private void _setupEvents(IApplicationBuilder app, IHostingEnvironment env)
        {
             
            var Context = app.ApplicationServices.GetService<MessageContext>();
            var dbContextOptions = new DbContextOptionsBuilder<MessageContext>().UseSqlServer(Configuration.GetConnectionString("NotificationDatabase")).Options;
            var _rawRabbitClient = app.ApplicationServices.GetService<IBusClient>();
            var configSendMail = app.ApplicationServices.GetRequiredService<IOptions<ConfigSendEmail>>();


            var _accountCreatedHandle = new AccountCreatedHandlers(_rawRabbitClient, env, configSendMail, dbContextOptions);
            _rawRabbitClient.SubscribeAsync<AccountCreatedForEmail>(_accountCreatedHandle.HandleAsync);

            var _requestGorgotPasswordHandle = new RequestForgotPasswordForEmailHandler(_rawRabbitClient, env, configSendMail, dbContextOptions);
            _rawRabbitClient.SubscribeAsync<RequestForgotPasswordForEmail>(_requestGorgotPasswordHandle.HandleAsync);
        }
    }
}
