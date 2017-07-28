using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ProfileService.Model
{
    public class ProfileContext : DbContext
    {
        public ProfileContext(DbContextOptions<ProfileContext> options) : base(options)
        {
           
        }

        public ProfileContext()
        {
        }

        public  DbSet<Employee> Employees { get; set; }
        public  DbSet<Team> Teams { get; set; }
        public  DbSet<Position> Positions { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
             
            base.OnModelCreating(modelBuilder);
        }
        public static void UpdateDatabase(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetRequiredService<ProfileContext>();
            
            context.Database.MigrateAsync();

            Seed(context);
        }

        private static void Seed(ProfileContext context)
        {
           
        }
       
    }
}
