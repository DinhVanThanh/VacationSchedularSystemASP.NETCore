using AuthenticationServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Setup
{
    public static class ConfigureSystemAdminTask
    {
        public static void ConfigureSystemAdmin(this IApplicationBuilder app)
        {
            var _context = app.ApplicationServices.GetRequiredService<AuthContext>();

            var sysadmin = _context.Accounts.Include(a => a.AccountPermissions).FirstOrDefault(a => a.UserName == "sysadmin@jobhop.com");

            if (sysadmin == null)
            {
                sysadmin = new Account
                {
                    UserName = "sysadmin@jobhop.com",
                    AccountPermissions = _context.Permissions.Select(p => new AccountPermission { PermissionId = p.Id }).ToList(),
                    IsSystemAdmin = true,
                    AccountType = JobHop.Common.Enums.AccountType.Admin
                };

                var _pwdHasher = new PasswordHasher<Account>();

                var hashedPassword = _pwdHasher.HashPassword(sysadmin, "demo@123");

                sysadmin.Password = hashedPassword;

                sysadmin.SecurityStamp = Guid.NewGuid().ToString("D");

                var now = DateTime.Now;

                sysadmin.CreatedAt = now;
                sysadmin.ModifiedAt = now;

                _context.Accounts.Add(sysadmin);
            }
            else
            {
                var hadPermissions = _context.AccountsPermissions.Where(a => a.AccountId == sysadmin.Id).Select(a => a.PermissionId);
                var missingPermissions = _context.Permissions.Where(p => !hadPermissions.Contains(p.Id)).Select(p => p.Id);
                sysadmin.AccountPermissions.AddRange(missingPermissions.Select(p => new AccountPermission { AccountId = sysadmin.Id, PermissionId = p }));
                sysadmin.AccountType = JobHop.Common.Enums.AccountType.Admin;
            }
            _context.SaveChanges();
        }
    }
}
