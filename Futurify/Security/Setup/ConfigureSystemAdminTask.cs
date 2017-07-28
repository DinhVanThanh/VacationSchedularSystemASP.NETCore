using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Security.Models;
using Vacation.common.Enums;

namespace Security.Setup
{
    public static class ConfigureSystemAdminTask
    {
        public static void ConfigureSystemAdmin(this IApplicationBuilder app)
        {
            var _context = app.ApplicationServices.GetRequiredService<AuthContext>();

            var sysadmin = _context.Accounts.Include(a => a.AccountPermissions).FirstOrDefault(a => a.UserName == "sysadmin@gmail.vn");

            if (sysadmin == null)
            {
                sysadmin = new Account
                {
                    UserName = "sysadmin@gmail.com",
                    AccountPermissions = _context.Permissions.Select(p => new AccountPermission { PermissionId = p.PermissionId }).ToList(),
                    IsSystemAdmin = true,
                   Type = AccountType.Admin
                };

                var _pwdHasher = new PasswordHasher<Account>();

                var hashedPassword = _pwdHasher.HashPassword(sysadmin, "admin@123");

                sysadmin.Password = hashedPassword;

                

                var now = DateTime.Now;

                sysadmin.CreatedAt = now;
                sysadmin.ModifiedAt = now;

                _context.Accounts.Add(sysadmin);
            }
            else
            {
                var hadPermissions = _context.AccountsPermissions.Where(a => a.AccountId == sysadmin.AccountId).Select(a => a.PermissionId);
                var missingPermissions = _context.Permissions.Where(p => !hadPermissions.Contains(p.PermissionId)).Select(p => p.PermissionId);
                sysadmin.AccountPermissions.AddRange(missingPermissions.Select(p => new AccountPermission { AccountId = sysadmin.AccountId, PermissionId = p }));
                sysadmin.Type = AccountType.Admin;
            }
            _context.SaveChanges();
        }
    }
}
