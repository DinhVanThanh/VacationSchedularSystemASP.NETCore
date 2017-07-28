using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AuthenticationServer.Providers;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using App.Common.Core.Models;
using AuthenticationServer.Models;
using AuthenticationServer.ServicesInterfaces;
using AuthenticationServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using App.Common.Core;
using RawRabbit.vNext;
using Swashbuckle.AspNetCore.Swagger;
using App.Common.Core.Authentication;
using App.Common.Core.Exceptions;
using AuthenticationServer.Setup;
using Newtonsoft.Json.Serialization;
using JobHop.Common.Options;
using AuthenticationServer.Models.BindingModels;

namespace AuthenticationServer
{
    public class Startup
    {
        private string jwtSecretToken, jwtAudience, jwtIssuer;
        private int jwtExpiresInDays;

        private string _contentRootPath;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            var jwtConfigs = Configuration.GetSection("Authentication").GetSection("JWT");

            jwtSecretToken = jwtConfigs["SecretKey"];
            jwtAudience = jwtConfigs["Audience"];
            jwtIssuer = jwtConfigs["Issuer"];
            jwtExpiresInDays = int.Parse(jwtConfigs["ExpiresInDays"]);

            _contentRootPath = env.ContentRootPath;
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TokenProviderOptions>(options =>
            {
                // secretKey contains a secret passphrase only your server knows
                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.jwtSecretToken));
                options.Audience = jwtAudience;
                options.Issuer = jwtIssuer;
                options.Expiration = TimeSpan.FromDays(jwtExpiresInDays);
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.Configure<FacebookProviderOptions>(Configuration.GetSection("Authentication").GetSection("Facebook"));
            services.Configure<EmailTemplate>(Configuration.GetSection("EmailTemplate"));

            //config send email for unhandled exception
            var emails = Configuration.GetSection("Administrators:Emails").Value;
            if (emails != null)
            {
                services.Configure<UnhandleExceptionEmailConfiguration>(options =>
                {
                    options.Sender = Configuration.GetSection("ConfigSendEmail:Sender").Value;
                    options.Receivers = emails.Split(',').ToList();
                    options.Receivers.ForEach(t => t.Trim());
                    options.Username = Configuration.GetSection("ConfigSendEmail:Username").Value;
                    options.Password = Configuration.GetSection("ConfigSendEmail:Password").Value;
                    options.Host = Configuration.GetSection("ConfigSendEmail:Host").Value;
                    options.Port = Configuration.GetSection("ConfigSendEmail:Port").Value;
                });
            }
          

            services.AddDbContext<AuthContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IVerificationService, VerificationService>();
            services.AddScoped<IESMSService, ESMSService>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAllOrigins"));
            });

            services.Configure<BananaOptions>(Configuration.GetSection("BananaOptions"));

            //config sms services
            var twilioConfigs = Configuration.GetSection("Twilio");
            services.Configure<TwilioProviderOptions>(twilioConfigs);
            Twilio.TwilioClient.Init(twilioConfigs["AccountSID"], twilioConfigs["AuthToken"]);

            //config esms services
            var esmsConfigs = Configuration.GetSection("eSMS");
            services.Configure<ESMSProviderOptions>(esmsConfigs);

            //config raw rabbit
            services.AddRawRabbit(cfg => cfg.SetBasePath(_contentRootPath).AddJsonFile("rabbitmq.json"));

            // Add framework services.
            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Authentication API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            AuthContext.UpdateDatabase(app);

            app.ConfigurePermissions();
            //system admin must be configure after permissions to ensure sysadmin has full permissions
            app.ConfigureSystemAdmin();

            app.UseCors("AllowAllOrigins");

            app.UseCustomExceptionHandler();

            //config using jwt authentication
            app.UseJwt(this.jwtSecretToken, this.jwtAudience, this.jwtIssuer);
            //config jwt provider middleware
            app.UseMiddleware<TokenProviderMiddleware>();
            //config unhandled exception
            app.UseMiddleware<UnhandlingException>();

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authentication V1");
            });
        }
    }
}
