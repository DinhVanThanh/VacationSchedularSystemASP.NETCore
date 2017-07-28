using AuthenticationServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AuthenticationServer.ServicesInterfaces;
using Microsoft.EntityFrameworkCore;
using App.Common.Core.Exceptions;
using AuthenticationServer.Resources;
using JobHop.Common.Helpers;
using JobHop.Common.Models;
using App.Common.Core;
using AuthenticationServer.Models.BindingModels;
using PhoneNumbers;

namespace AuthenticationServer.Services
{
    public class AccountService : IAccountService
    {
        AuthContext _context;
        PasswordHasher<Account> _pwdHasher;

        public AccountService(AuthContext context)
        {
            _context = context;
            _pwdHasher = new PasswordHasher<Account>();
        }

        public async Task<Account> CheckExsitByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Accounts.Include(s => s.VerificationCodes).FirstOrDefaultAsync(t => t.PhoneNumber == phoneNumber);
        }

        public async Task<Account> CheckExsitByUserNameAsync(string userName)
        {
            return await _context.Accounts.FirstOrDefaultAsync(t => t.Email == userName);
        }

        public async Task<Account> CheckAsync(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var account = await this.FindByUserNameAsync(userName, userName.Contains("@") ? UserNameType.Email : UserNameType.PhoneNumber);
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
                return await this.UpdatePasswordAsync(account.Id, password);
            }
            else
            {
                return account;
            }
        }

        public async Task<Account> CreateAsync(Account account, string password, bool forExternal = false)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            if (string.IsNullOrEmpty(account.UserName) || (!forExternal && string.IsNullOrEmpty(password)))
            {
                throw new CustomException(Errors.INVALID_REGISTRATION_DATA, Errors.INVALID_REGISTRATION_DATA_MSG);
            }

            var existUserName = await this.FindByUserNameAsync(account.UserName);

            if (existUserName != null)
            {
                if (existUserName.AccountType == JobHop.Common.Enums.AccountType.Jobseeker && !existUserName.PhoneNumberVerified)
                {
                    throw new CustomException(Errors.WAIT_FOR_VERIFICATION, Errors.WAIT_FOR_VERIFICATION_MSG);
                }
                else
                {
                    throw new CustomException(Errors.USERNAME_ALREADY_IN_USE, Errors.USERNAME_ALREADY_IN_USE_MSG);
                }
            }

            var existUserEmail = await this.FindByEmailAsync(account.Email);
            if (existUserEmail != null)
            {
                throw new CustomException(Errors.EMAIL_ALREADY_IN_USE, Errors.EMAIL_ALREADY_IN_USE_MSG);
            }

            if (!string.IsNullOrEmpty(password))
            {
                var hashedPassword = _pwdHasher.HashPassword(account, password);

                account.Password = hashedPassword;

            }
            account.SecurityStamp = AccountService.GenerateSecurityStamp();

            var now = DateTime.Now;

            account.CreatedAt = now;
            account.ModifiedAt = now;

            _context.Accounts.Add(account);

            account.VerificationCodes = new List<VerificationCode>();

            //modified create account method to validate worker phone number
            if (account.AccountType == JobHop.Common.Enums.AccountType.Jobseeker)
            {
                account.VerificationCodes.Add(new VerificationCode
                {
                    SetPhoneNumber = account.PhoneNumber,
                    ExpiredAt = DateTime.Now.AddDays(1),
                    Purpose = VerificationPurpose.RegistrationPhoneNumber,
                    VerifyCode = CommonFunctions.GenerateVerificationCode(true)
                });
            }

            await _context.SaveChangesAsync();

            return account;
        }

        public async Task<Account> UpdatePasswordAsync(int accountId, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            var account = await this.FindByIdAsync(accountId);
            if (account == null)
            {
                throw new CustomException(Errors.ACCOUNT_NOT_FOUND, Errors.ACCOUNT_NOT_FOUND_MSG);
            }

            var hashedPassword = _pwdHasher.HashPassword(account, password);

            account.Password = hashedPassword;

            account.SecurityStamp = AccountService.GenerateSecurityStamp();

            await _context.SaveChangesAsync();

            return account;
        }

        public async Task ChangePasswordAsync(int accountId, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(oldPassword))
            {
                throw new ArgumentNullException("oldPassword");
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentNullException("newPassword");
            }

            var account = await this.FindByIdAsync(accountId);
            if (account == null)
            {
                throw new CustomException(Errors.ACCOUNT_NOT_FOUND, Errors.ACCOUNT_NOT_FOUND_MSG);
            }

            var checkedAccount = await this.CheckAsync(account.UserName, oldPassword);
            if (checkedAccount == null)
            {
                throw new CustomException(Errors.OLD_PASSWORD_INCORRECT, Errors.OLD_PASSWORD_INCORRECT_MSG);
            }

            await this.UpdatePasswordAsync(accountId, newPassword);
        }

        public async Task<Account> FindByEmailAsync(string email, bool? checkVerified = null)
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
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Account> FindByUserNameAsync(string userName, UserNameType type = UserNameType.PhoneNumber)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }

            if (type == UserNameType.PhoneNumber)
            {
                return await _context.Accounts.FirstOrDefaultAsync(a => a.UserName.ToLower() == userName.ToLower());
            }
            else
            {
                return await _context.Accounts.FirstOrDefaultAsync(a => a.UserName.ToLower() == userName.ToLower() || a.Email == userName);
            }
        }

        public async Task<Account> FindByExternalProviderAsync(string provider, string providerKey)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.ExternalLogins.Any(e => e.Provider.ToLower() == provider.ToLower() && providerKey.ToLower() == e.ProviderKey.ToLower()));
        }

        public async Task<IEnumerable<string>> GetPermissionsOfAccountAsync(int accountId)
        {
            return await _context.AccountsRoles.Where(a => a.AccountId == accountId).SelectMany(a => a.Role.RolePermissions.Select(p => p.PermissionId))
                .Concat(_context.AccountsPermissions.Where(a => a.AccountId == accountId).Select(a => a.PermissionId)).Distinct().ToListAsync();
        }

        public async Task<List<Role>> GetRolesOfAccountAsync(int accountId)
        {
            return await _context.Roles.Where(r => r.AccountRoles.Any(a => a.AccountId == accountId)).ToListAsync();
        }

        private static string GenerateSecurityStamp()
        {
            return Guid.NewGuid().ToString("D");
        }

        public async Task SetRoleAsync(int accountId, int roleId)
        {
            if (!_context.Accounts.Any(a => a.Id == accountId))
            {
                throw new CustomException(Errors.ACCOUNT_NOT_FOUND, Errors.ACCOUNT_NOT_FOUND_MSG);
            }

            if (!_context.Roles.Any(r => r.Id == roleId))
            {
                throw new CustomException(Errors.ROLE_NOT_FOUND, Errors.ROLE_NOT_FOUND_MSG);
            }

            if (!_context.AccountsRoles.Any(a => a.AccountId == accountId && a.RoleId == roleId))
            {
                _context.AccountsRoles.Add(new AccountRole
                {
                    AccountId = accountId,
                    RoleId = roleId
                });

                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveRoleAsync(int accountId, int roleId)
        {
            var accountRole = await _context.AccountsRoles.FirstOrDefaultAsync(a => a.AccountId == accountId && a.RoleId == roleId);

            if (accountRole != null)
            {
                _context.AccountsRoles.Remove(accountRole);

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteRecruiter(int id)
        {
            var existAccount = await FindByIdAsync(id);
            if (existAccount == null)
            {
                throw new CustomException(Errors.ACCOUNT_NOT_FOUND, Errors.ACCOUNT_NOT_FOUND_MSG);
            }

            if (existAccount.AccountType != JobHop.Common.Enums.AccountType.Recruiter)
            {
                throw new CustomException(Errors.INVALID_REQUEST, Errors.INVALID_REQUEST_MSG);
            }

            _context.Accounts.Remove(existAccount);

            await _context.SaveChangesAsync();
        }

        public async Task SetStatusAsync(int accountId, UserStatus status)
        {
            var account = await FindByIdAsync(accountId);
            if (account == null)
            {
                throw new CustomException(Errors.ACCOUNT_NOT_FOUND, Errors.ACCOUNT_NOT_FOUND_MSG);
            }

            account.Status = status;

            await _context.SaveChangesAsync();
        }

        public async Task<LoginTracker> GetAsyncLoginTracker(int trackerId)
        {
            return await _context.LoginTrackers.FindAsync(trackerId);
        }

        public async Task<LoginTracker> AddTracker(LoginTracker tracker)
        {
            if (tracker == null)
                return null;

            var loginAt = tracker.LoginAt.Date;
            var existing = await _context.LoginTrackers.FirstOrDefaultAsync(t => t.Account.Id == tracker.Account.Id && t.LoginAt >= loginAt);
            if (existing != null)
                return null;

            _context.LoginTrackers.Add(tracker);
            await _context.SaveChangesAsync();

            return tracker;
        }

        public async Task<RequestResetPassword> CreateRequestResetPassword(RequestResetPassword request)
        {
            var existing = await _context.RequestResetPasswords.FirstOrDefaultAsync(t => t.Email == request.Email && t.ExpiredAt > DateTime.Now);
            if (existing != null)
                return existing;

            request.Token = GenerateSecurityStamp();
            _context.RequestResetPasswords.Add(request);
            await _context.SaveChangesAsync();

            return request;
        }

        public async Task ResetPasswordByPhoneNumber(PhoneNumberResetPassword model)
        {
            string formatedPhoneNumber = PhoneNumberHelpers.GetFormatedPhoneNumber(model.PhoneNumber);
            var account = await CheckExsitByPhoneNumberAsync(formatedPhoneNumber);

            if (account == null)
            {
                throw new CustomException(Errors.ACCOUNT_NOT_FOUND, Errors.ACCOUNT_NOT_FOUND_MSG);
            }

            var verification = account.VerificationCodes.FirstOrDefault(t => t.SetPhoneNumber == formatedPhoneNumber && t.Purpose == VerificationPurpose.Password && t.Checked);
            if (verification == null)
            {
                throw new CustomException(Errors.PIN_NOT_VERIFY, Errors.PIN_NOT_VERIFY_MSG);
            }

            account.Password = _pwdHasher.HashPassword(account, model.NewPassword);
            account.SecurityStamp = GenerateSecurityStamp();
            account.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task ResetPasswordByEmail(EmailResetPassword model)
        {
            var account = await CheckExsitByUserNameAsync(model.Email);
            if (account == null)
            {
                throw new CustomException(Errors.ACCOUNT_NOT_FOUND, Errors.ACCOUNT_NOT_FOUND_MSG);
            }

            var now = DateTime.Now;
            var request = await _context.RequestResetPasswords.FirstOrDefaultAsync(s => s.Email == model.Email && s.Token == model.Token && s.ExpiredAt > now);
            if(request == null)
            {
                throw new CustomException(Errors.TOKEN_IS_EXPIRED, Errors.TOKEN_IS_EXPIRED_MSG);
            }

            account.Password = _pwdHasher.HashPassword(account, model.NewPassword);
            account.SecurityStamp = GenerateSecurityStamp();
            account.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}
