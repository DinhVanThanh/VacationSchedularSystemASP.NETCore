using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Notification.Models
{
    public class MessageContext : DbContext
    {
        public MessageContext(DbContextOptions<MessageContext> options) : base(options)
        {
        }
        public MessageContext()
        {
        }
         
       
        public virtual DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Skip shadow types
                if (entityType.ClrType == null)
                {
                    continue;
                }

                entityType.Relational().TableName = entityType.ClrType.Name;
            }

            base.OnModelCreating(modelBuilder);
        }
        public static void UpdateDatabase(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetRequiredService<MessageContext>();

            context.Database.Migrate();
            MessageContext.Seed(context);

        }

        private static void Seed(MessageContext context)
        {

        }
    }
}
