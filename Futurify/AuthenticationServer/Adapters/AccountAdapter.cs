using App.common.core.Exceptions;
using AuthenticationServer.Models;
using AuthenticationServer.Models.BindingModels;
using AuthenticationServer.Models.ViewModels;
using AuthenticationServer.Resources;
using PhoneNumbers;

namespace AuthenticationServer.Adapters
{
    public static class AccountAdapter
    {
        public static AccountViewModel ToViewModel(this Account model)
        {
            if (model == null)
            {
                return null;
            }

            var viewModel = new AccountViewModel
            {
                CreatedAt = model.CreatedAt,
                CreatedBy = model.CreatedBy.ToViewModel(),
                Email = model.Email,
                EmailVerified = model.EmailVerified,
                Id = model.Id,
                LockBy = model.LockBy.ToViewModel(),
                Locked = model.Locked,
                LockedAt = model.LockedAt,
                LockUntil = model.LockUntil,
                ModifiedAt = model.ModifiedAt,
                ModifiedBy = model.ModifiedBy.ToViewModel(),
                PhoneNumber = model.PhoneNumber,
                PhoneNumberVerified = model.PhoneNumberVerified,
                UserName = model.UserName
            };

            return viewModel;
        }

        public static Account RegisterModelToModel(this RegisterModel model)
        {
            if (model == null)
            {
                return null;
            }

            var account = new Account
            {
                AccountType = JobHop.Common.Enums.AccountType.Jobseeker,
                PhoneNumber = model.PhoneNumber,
                UserName = model.PhoneNumber
            };

            return account;
        }

        public static Account CreateRecruiterModelToAccountModel(this CreateRecruiterModel model)
        {
            if (model == null)
            {
                return null;
            }

            var account = new Account
            {
                AccountType = JobHop.Common.Enums.AccountType.Recruiter,
                Email= model.Email,
                Status = UserStatus.Active
            };


            try
            {
                //standardize phone number
                var formatedPhoneNumber = PhoneNumberHelpers.GetFormatedPhoneNumber(model.PhoneNumber);
                account.PhoneNumber = formatedPhoneNumber;
                account.UserName = formatedPhoneNumber;
            }
            catch (NumberParseException)
            {
                throw new CustomException(Errors.INVALID_PHONE_NUMBER, Errors.INVALID_PHONE_NUMBER_MSG);
            }

            return account;
        }

    }
}
