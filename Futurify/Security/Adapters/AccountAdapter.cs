using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Security.Models;
using Security.Models.BindingModels;
using Security.Models.ViewModels;
using System.Security;
using App.common.core.Helpers;
using App.common.core.Exceptions;

namespace Security.Adapters
{
    public static class AccountAdapter
    {
        public static AccountViewModel ToViewModel( Account model)
        {
            if (model == null)
            {
                return null;
            }
            try
            {
                var viewModel = new AccountViewModel
                {
                    CreatedAt = model.CreatedAt,
                    CreatedBy = AccountAdapter.ToViewModel(model.CreatedBy),
                    Email = model.Email,
                    EmailVerified =  model.EmailVerified.HasValue ? model.EmailVerified.Value : false,
                    Id = model.AccountId,
                    LockBy = AccountAdapter.ToViewModel(model.LockBy),
                    Locked = model.Locked.HasValue ? model.Locked.Value : false,
                    LockedAt = model.LockedAt,
                    LockUntil = model.LockUntil,
                    ModifiedAt = model.ModifiedAt,
                    ModifiedBy = AccountAdapter.ToViewModel(model.ModifiedBy),
                    PhoneNumber = model.PhoneNumber,
                    PhoneNumberVerified = model.PhoneNumberVerified.HasValue ? model.PhoneNumberVerified.Value : false,
                    UserName = model.UserName
                };
                return viewModel;
            }
            catch(Exception e)
            {
                throw new CustomException(e.Message, e.InnerException.Message); 
            }
             
            
        }

        public static Account RegisterModelToModel(RegisterModel model)
        {
            if (model == null)
            {
                return null;
            }

            var account = new Account
            {
                Status = Vacation.common.Enums.UserStatus.Pending, 
                UserName = model.Email,
                Email = model.Email,
                Password = PasswordHelper.CreateDefaultPassword(7)
            };

            return account;
        }

       
    }
}
