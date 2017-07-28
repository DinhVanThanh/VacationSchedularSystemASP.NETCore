using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.common.core;
using App.common.core.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Security.Helpers;
using Security.IServiceInterfaces;
using Security.Models;
using Security.Options;
using Vacation.common.Enums;

namespace Security.Services
{
    public class AccountService : IAccountService
    {
        
        private AuthContext _context;
        private PasswordHasher<Account> _pwdHasher;
        public AccountService(AuthContext context)
        {
           
            _context = context;
            _pwdHasher = new PasswordHasher<Account>();
        }
        public async Task ChangePasswordAsync(int accountId, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public async Task<Account> CheckAsync(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var account = await this.FindByUserNameAsync(userName);
            if (account == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(account.Password))
            {
                return null;
            }

            var hashedPassword = account.Password;

            var result = _pwdHasher.VerifyHashedPassword(account, hashedPassword, password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }
            else if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                return await this.UpdatePasswordAsync(account.AccountId, password);
            }
            else
            {
                return account;
            }
        }

        public Task<Account> CheckExsitByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<Account> CheckExsitByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Accounts.Include(s => s.VerificationCodes).FirstOrDefaultAsync(t => t.PhoneNumber == phoneNumber);
        }

        public async Task<Account> CheckExsitByUserNameAsync(string userName)
        {
            return await _context.Accounts.FirstOrDefaultAsync(t => t.Email == userName);
        }

        public async Task<Account> CreateAsync(Account account, string password)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            if (string.IsNullOrEmpty(account.UserName) || string.IsNullOrEmpty(password))
            {
                throw new CustomException("Invalid Registration Data", "Invalid Registration Data Message");
            }

            var existUserName = await this.FindByUserNameAsync(account.UserName);

            if (existUserName != null)
            {
                if (!(bool)existUserName.PhoneNumberVerified)
                {
                    throw new CustomException("WAIT_FOR_VERIFICATION", "WAIT_FOR_VERIFICATION_MSG");
                }
                else
                {
                    throw new CustomException("USERNAME_ALREADY_IN_USE", "USERNAME_ALREADY_IN_USE_MSG");
                }
            }

            var existUserEmail = await this.FindByEmailAsync(account.Email, null);
            if (existUserEmail != null)
            {
                throw new CustomException("EMAIL_ALREADY_IN_USE", "EMAIL_ALREADY_IN_USE_MSG");
            }

            if (!string.IsNullOrEmpty(password))
            {
                var hashedPassword = _pwdHasher.HashPassword(account, password);

                account.Password = hashedPassword;

            }
            

            var now = DateTime.Now;

            account.CreatedAt = now;
            account.ModifiedAt = now;

            _context.Accounts.Add(account);

            account.VerificationCodes = new List<VerificationCode>();

            //modified create account method to validate worker phone number
            
            account.VerificationCodes.Add(new VerificationCode
            {
                CodeReceiver = account.PhoneNumber,
                ExpiredAt = DateTime.Now.AddDays(1),
                Purpose = VerificationPurpose.RegistrationPhoneNumber,
                Code = CommonFunctions.GenerateVerificationCode(true)
            });

            try
            {
                _context.SaveChanges();
            }
            catch(Exception e)
            {
                throw new CustomException(e.Message, e.Message);
            }
             

            return account;
        }

        public async Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Account> FindByEmailAsync(string email, bool? checkVerified)
        {
            if (email == null)
            {
                return null;
            }

            if (checkVerified.HasValue)
            {
                return await _context.Accounts.FirstOrDefaultAsync(a => a.Email.ToLower() == email.ToLower() && a.EmailVerified == checkVerified.Value);
            }
            else
            {
                return await _context.Accounts.FirstOrDefaultAsync(a => a.Email.ToLower() == email.ToLower());
            }
        }

        public async Task<Account> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Account> FindByUserNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }
            return await _context.Accounts.FirstOrDefaultAsync(a => a.UserName.ToLower() == userName.ToLower() || a.Email == userName);
            
        }

        public async Task<IEnumerable<string>> GetPermissionsOfAccountAsync(int accountId)
        {
            return await _context.AccountsRoles.Where(a => a.AccountId == accountId).SelectMany(a => a.Role.RolePermissions.Select(p => p.PermissionId))
                .Concat(_context.AccountsPermissions.Where(a => a.AccountId == accountId).Select(a => a.PermissionId)).Distinct().ToListAsync();
        }

        public async Task<List<Role>> GetRolesOfAccountAsync(int accountId)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveRoleAsync(int accountId, int roleId)
        {
            throw new NotImplementedException();
        }

       
        public async Task SetRoleAsync(int accountId, int roleId)
        {
            throw new NotImplementedException();
        }

        public async Task SetStatusAsync(int accountId, UserStatus status)
        {
            throw new NotImplementedException();
        }

        public async Task<Account> UpdatePasswordAsync(int accountId, string password)
        {
            throw new NotImplementedException();
        }
    }
}
