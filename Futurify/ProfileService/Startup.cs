using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProfileService.Model;

using Microsoft.EntityFrameworkCore;
using ProfileService.IServiceInterfaces;
using ProfileService.Services;
using RawRabbit.Extensions.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using ProfileService.EventHandlers;
using Vacation.common.Events;

namespace ProfileService
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
            // Add framework services.
            services.AddDbContext<ProfileContext>(options => options.UseSqlServer(Configuration.GetSection("ConnectionStrings").GetSection("VacationDatabase").Value));
            services.AddRawRabbit(cfg => cfg.SetBasePath(_contentRootPath).AddJsonFile("rabbitmq.json"));
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<ITeamService, TeamSevice>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAllOrigins"));
            });
            services.AddRawRabbit(cfg => cfg.SetBasePath(_contentRootPath).AddJsonFile("rabbitmq.json"));
             
            services.AddMvc();
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors("AllowAllOrigins");
            app.UseStaticFiles();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            ProfileContext.UpdateDatabase(app);
            app.UseMvc();

            var _rawRabbitClient = app.ApplicationServices.GetService<IBusClient>();

            _setupEvents(app, env);
        }

        private void _setupEvents(IApplicationBuilder app, IHostingEnvironment env)
        {
            var Context = app.ApplicationServices.GetService<ProfileContext>();
            var dbContextOptions = new DbContextOptionsBuilder<ProfileContext>().UseSqlServer(Configuration.GetConnectionString("VacationDatabase")).Options;
            var _rawRabbitClient = app.ApplicationServices.GetService<IBusClient>(); 
            var _profileHandle = new AccountCreatedHandlers(dbContextOptions);
            _rawRabbitClient.SubscribeAsync<AccountCreatedForEmail>(_profileHandle.HandleAsync);
        }
    }
}
