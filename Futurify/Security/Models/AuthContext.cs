using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Security.Models
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
        }

        public AuthContext()
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<AccountRole> AccountsRoles { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<AccountPermission> AccountsPermissions { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; } 
        public  DbSet<VerificationCode> VerificationCodes { get; set; } 

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

            modelBuilder.Entity<Account>().HasOne(a => a.LockBy);
            modelBuilder.Entity<Account>().HasOne(a => a.CreatedBy);
            modelBuilder.Entity<Account>().HasOne(a => a.ModifiedBy);

            modelBuilder.Entity<AccountRole>().HasKey("AccountId", "RoleId");
            modelBuilder.Entity<RolePermission>().HasKey("RoleId", "PermissionId");
            modelBuilder.Entity<AccountPermission>().HasKey("AccountId", "PermissionId");

            base.OnModelCreating(modelBuilder);
        }

        public static void UpdateDatabase(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetRequiredService<AuthContext>();

            context.Database.Migrate();

            AuthContext.Seed(context);
        }

        private static void Seed(AuthContext context)
        {
            //seed code
        }
    }
}
