using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models
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
        public virtual DbSet<ExternalProvider> ExternalProviders { get; set; }
        public virtual DbSet<VerificationCode> VerificationCodes { get; set; }
        public virtual DbSet<LoginTracker> LoginTrackers { get; set; }
        public virtual DbSet<RequestResetPassword> RequestResetPasswords { get; set; }

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
            
            modelBuilder.Entity<ExternalProvider>().HasKey("Provider", "ProviderKey");
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
