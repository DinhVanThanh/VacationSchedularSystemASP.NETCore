using JobHop.Common.Models;
using AuthenticationServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationServer.Models.BindingModels;

namespace AuthenticationServer.ServicesInterfaces
{
    public interface IAccountService
    {
        Task<Account> CheckExsitByPhoneNumberAsync(string phoneNumber);
        Task<Account> CheckExsitByUserNameAsync(string userName);
        Task<Account> CheckAsync(string userName, string password);
        Task<Account> CreateAsync(Account account, string password, bool forExternal = false);
        Task<Account> FindByEmailAsync(string email, bool? checkVerified);
        Task<Account> FindByIdAsync(int id);
        Task<Account> FindByUserNameAsync(string userName, UserNameType type = UserNameType.PhoneNumber);
        Task<Account> FindByExternalProviderAsync(string provider, string providerKey);
        Task<IEnumerable<string>> GetPermissionsOfAccountAsync(int accountId);
        Task<List<Role>> GetRolesOfAccountAsync(int accountId);
        Task<Account> UpdatePasswordAsync(int accountId, string password);
        Task ChangePasswordAsync(int accountId, string oldPassword, string newPassword);
        Task SetRoleAsync(int accountId, int roleId);
        Task RemoveRoleAsync(int accountId, int roleId);
        Task DeleteRecruiter(int id);
        Task SetStatusAsync(int accountId, UserStatus status);
        Task<LoginTracker> GetAsyncLoginTracker(int trackerId);
        Task<LoginTracker> AddTracker(LoginTracker tracker);
        Task<RequestResetPassword> CreateRequestResetPassword(RequestResetPassword request);
        Task ResetPasswordByPhoneNumber(PhoneNumberResetPassword model);
        Task ResetPasswordByEmail(EmailResetPassword model);
    }
}
