using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.common.core.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Security.Models;
using Security.Providers;
using Microsoft.EntityFrameworkCore;
using Security.IServiceInterfaces;
using Security.Services;
using Security.Setup;
using RawRabbit.vNext;
using Security.Options;

namespace Security
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
            services.AddDbContext<AuthContext>(options => options.UseSqlServer(Configuration.GetSection("ConnectionStrings").GetSection("AuthDatabase").Value));
            services.Configure<JWTSettings>(Configuration.GetSection("JWTSettings"));
            services.Configure<ResetPasswordOptions>(Configuration.GetSection("ResetPasswordSettings"));
            services.AddRawRabbit(cfg => cfg.SetBasePath(_contentRootPath).AddJsonFile("rabbitmq.json"));
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRoleService, RoleService>(); 
            services.AddScoped<IVerificationService, VerificationService>();
            
            services.AddScoped<IResetPasswordService, ResetPasswordService>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAllOrigins"));
            });
            
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //AuthContext.UpdateDatabase(app);

            app.ConfigurePermissions();

            app.ConfigureSystemAdmin();

            app.UseCors("AllowAllOrigins");
            app.UseMiddleware<TokenProviderMiddleware>();
           
            
            app.UseMvc();
        }
    }
}
