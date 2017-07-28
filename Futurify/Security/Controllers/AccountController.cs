using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.common.core.Authentication;
using App.common.core.Exceptions;
using App.common.core.Helpers;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PhoneNumbers;
using Security.Adapters;
using Security.IServiceInterfaces;
using Security.Models.BindingModels;
using System.Security.Claims;
using RawRabbit;
using Vacation.common.Events;
using Vacation.common;

namespace Security.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private IResetPasswordService _resetpasswordService;
        private IBusClient _rawRabbitBus;
        private IAccountService _accountService; 
        public AccountController(IAccountService accountService, IBusClient rawRabbitBus, IResetPasswordService resetpasswordService)
        {
            _resetpasswordService = resetpasswordService;
            _rawRabbitBus = rawRabbitBus;
            _accountService = accountService;
        }
        // GET api/values
        [HttpGet]
        [Route("me/permissions/{id}")]
        public async Task<IEnumerable<string>> GetPermissions(int id)
        {
            IEnumerable<string> permissions = await _accountService.GetPermissionsOfAccountAsync(id);
            return permissions;
        }

        // GET api/values/5
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterModel registerModel)
        {
            var account = AccountAdapter.RegisterModelToModel(registerModel);
            if (account == null || !ModelState.IsValid)
            {
                throw new CustomException("Errors.INVALID_REGISTRATION_DATA", "Errors.INVALID_REGISTRATION_DATA_MSG");
            }

            try
            {
                if (registerModel.PhoneNumber != null)
                {
                    var formatedPhoneNumber = PhoneNumbers.PhoneNumberHelpers.GetFormatedPhoneNumber(registerModel.PhoneNumber);
                    account.PhoneNumber = formatedPhoneNumber;
                }
                //standardize phone number
                
            }
            catch (NumberParseException)
            {
                throw new CustomException("Errors.INVALID_PHONE_NUMBER", "Errors.INVALID_PHONE_NUMBER_MSG");
            }

            account.AccountPermissions = new List<Models.AccountPermission>()
            {
                 new Models.AccountPermission
                 {
                      PermissionId = "MEMBER"
                 }
            };
            string password = account.Password;
            account = await _accountService.CreateAsync(account, account.Password);
            //publish a jobseeker account is created message to rabbit mq bus
            await _rawRabbitBus.PublishAsync(new AccountCreatedForEmail { Id = account.AccountId, Password = password, Birthday = registerModel.Birthday, Position  = registerModel.Position, PhoneNumber = account.PhoneNumber, FirstName = registerModel.FirstName, LastName = registerModel.LastName, Email = registerModel.Email, LoginUrl = CommonContants.LoginUrl });

            //string smsContent = $"Verification code at JobHop: {account.VerificationCodes.First().VerifyCode}";

            ////send SMS using eSMS.vn
            //var response = await _esmsService.SendSMS(account.PhoneNumber, smsContent, 4);

            var viewModel = AccountAdapter.ToViewModel(account);

            
            return new JsonResult(viewModel);
        }

        // POST api/values
        [HttpGet]
        [Route("RequestResetPasswordByEmail/{Email}")]
        public async void RequestResetPasswordByEmail(string Email)
        {
            var existingAccount = await _accountService.CheckExsitByUserNameAsync(Email);

            if(existingAccount != null)
            {
               await _resetpasswordService.SendResetPasswordCodeAndUrl(Email);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
         
    }
}
