using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Report.Models
{
    public class ReportContext : DbContext
    {
        public ReportContext(DbContextOptions<ReportContext> options) : base(options)
        {

        }

        public ReportContext()
        {
        }

        public DbSet<VacationReport> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
        public static void UpdateDatabase(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetRequiredService<ReportContext>();

            context.Database.MigrateAsync();

            Seed(context);
        }
        public override int SaveChanges()
        {
            var currentTime = DateTime.Now;


            var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Property("CreatedOn") != null)
                    {
                        entry.Property("CreatedOn").CurrentValue = currentTime;
                    }
                    //if (entry.Property("CreatedBy") != null)
                    //{
                    //    if (currentUser != null)
                    //        entry.Property("CreatedBy").CurrentValue = currentUser.Id;
                    //    else if (entry.Property("CreatedBy").CurrentValue == null)
                    //        entry.Property("CreatedBy").CurrentValue = 0;
                    //}
                }

                if (entry.Property("ModifiedOn") != null)
                {
                    entry.Property("ModifiedOn").CurrentValue = currentTime;
                    //entry.Property("ModifiedBy").CurrentValue = currentUser != null ? currentUser.Id : 0;
                }
            }
            return base.SaveChanges();
        }
        private static void Seed(ReportContext context)
        {

        }
    }
}
