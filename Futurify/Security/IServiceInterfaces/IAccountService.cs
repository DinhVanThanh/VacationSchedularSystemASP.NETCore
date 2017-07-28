using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Security.Models;
using Vacation.common.Enums;

namespace Security.IServiceInterfaces
{
    public interface IAccountService
    {
        Task<Account> CheckExsitByPhoneNumberAsync(string phoneNumber);
        Task<Account> CheckExsitByUserNameAsync(string userName);
        Task<Account> CheckExsitByEmailAsync(string email);
        Task<Account> CheckAsync(string userName, string password);
        Task<Account> CreateAsync(Account account, string password);
        Task<Account> FindByEmailAsync(string email, bool? checkVerified);
        Task<Account> FindByIdAsync(int id);
        Task<Account> FindByUserNameAsync(string userName);
        
        Task<IEnumerable<string>> GetPermissionsOfAccountAsync(int accountId);
        Task<List<Role>> GetRolesOfAccountAsync(int accountId);
        Task<Account> UpdatePasswordAsync(int accountId, string password);
        Task ChangePasswordAsync(int accountId, string oldPassword, string newPassword);
        Task SetRoleAsync(int accountId, int roleId);
        Task RemoveRoleAsync(int accountId, int roleId);
        Task Delete(int id);
        Task SetStatusAsync(int accountId, UserStatus status);

        
    }
}
