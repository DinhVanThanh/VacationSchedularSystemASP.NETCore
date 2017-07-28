using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthenticationServer.Models.BindingModels;
using AuthenticationServer.ServicesInterfaces;
using App.Common.Core.Exceptions;
using AuthenticationServer.Resources;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using AuthenticationServer.Providers;
using Microsoft.Extensions.Options;
using AuthenticationServer.Services;
using PhoneNumbers;
using AuthenticationServer.Models;
using App.Common.Core;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthenticationServer.Controllers
{
    [Route("api/verification")]
    public class VerificationController : Controller
    {
        IAccountService _accountService;
        private IVerificationService _verificationService;
        private readonly IESMSService _esmsService;

        public VerificationController(IVerificationService verificationService, IESMSService esmsService, IAccountService accountService)
        {
            _verificationService = verificationService;
            _esmsService = esmsService;
            _accountService = accountService;
        }

        [HttpPost, Route("registration/phone-number")]
        public async Task<bool> VerifyRegistrationPhoneNumber([FromBody]VerifyPhoneNumberModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                throw new CustomException(Errors.INVALID_REQUEST, Errors.INVALID_REQUEST_MSG);
            }

            await _verificationService.VerifyRegistrationPhoneNumberAsync(model.PhoneNumber, model.PIN);

            return true;
        }

        [HttpPost, Route("registration/phone-number/re-send")]
        public async Task ResendVerifyCode([FromBody]SendVerificationCodeToPhoneNumberModel model)
        {
            var verification = await _verificationService.GetVerificationCodeFromPhoneNumberAsync(model.PhoneNumber, VerificationPurpose.RegistrationPhoneNumber);

            if (verification == null)
            {
                throw new CustomException(Errors.VERIFICATION_NOT_FOUND, Errors.VERIFICATION_NOT_FOUND_MSG);
            }
            // expired the verification if retried counter or resent counter exceeded limit
            // do not use >= for Resend because of we need increase Resend counter and expired immediatelly the verification
            else if (verification.Retry >= VerificationService.MAX_RETRY || verification.Resend > VerificationService.MAX_RESEND)
            {
                throw new CustomException(Errors.VERIFICATION_LOCKED, Errors.VERIFICATION_LOCKED_MSG);
            }
            else
            {
                if (verification.Resend == VerificationService.MAX_RESEND)
                {
                    await _verificationService.IncreaseResendCounter(verification.Id);
                    throw new CustomException(Errors.VERIFICATION_LOCKED, Errors.VERIFICATION_LOCKED_MSG);
                }
                else
                {
                    string smsContent = $"Verification code at JobHop: {verification.VerifyCode}";

                    var formatedPhoneNumber = PhoneNumberHelpers.GetFormatedPhoneNumber(model.PhoneNumber);
                    //send SMS using eSMS.vn
                    var response = await _esmsService.SendSMS(formatedPhoneNumber, smsContent, 4);

                    if (response.IsSuccessStatusCode)
                    {
                        await _verificationService.IncreaseResendCounter(verification.Id);
                    }
                    else
                    {
                        throw new CustomException(response.StatusCode.ToString());
                    }
                }
            }
        }

        [HttpGet, Route("reset-password/request")]
        public async Task RequestResetPassword([FromQuery]string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                throw new CustomException(Errors.INVALID_PHONE_NUMBER, Errors.INVALID_PHONE_NUMBER_MSG);
            }

            phoneNumber = PhoneNumberHelpers.GetFormatedPhoneNumber(phoneNumber);
            var account = await _accountService.CheckExsitByPhoneNumberAsync(phoneNumber);
            if (account == null)
            {
                throw new CustomException(Errors.ACCOUNT_NOT_FOUND, Errors.ACCOUNT_NOT_FOUND_MSG);
            }

            var verification = account.VerificationCodes.FirstOrDefault(t => t.SetPhoneNumber == phoneNumber && t.Purpose == VerificationPurpose.Password && !t.Checked);
            if (verification == null)
            {
                verification= new VerificationCode
                {
                    Account = account,
                    ExpiredAt = DateTime.Now.AddDays(1),
                    Purpose = VerificationPurpose.Password,
                    SetPhoneNumber = phoneNumber,
                    VerifyCode = CommonFunctions.GenerateVerificationCode(true)
                };

                await _verificationService.CreateAsync(verification);
            }

            string smsContent = $"Verification code at JobHop: {verification.VerifyCode}";

            //send SMS using eSMS.vn
            await _esmsService.SendSMS(account.PhoneNumber, smsContent, 4);
        }

        [HttpPost, Route("reset-password/verify")]
        public async Task<bool> VerifyResetPasswordPINCode([FromBody]PhoneNumberResetPassword model)
        {
            if (model == null)
            {
                throw new CustomException(Errors.REQUEST_NOT_NULL, Errors.REQUEST_NOT_NULL_MSG);
            }
            else if (string.IsNullOrEmpty(model.PhoneNumber))
            {
                throw new CustomException(Errors.INVALID_PHONE_NUMBER, Errors.INVALID_PHONE_NUMBER_MSG);
            }
            else if (string.IsNullOrEmpty(model.PIN))
            {
                throw new CustomException(Errors.PIN_NOT_NULL, Errors.PIN_NOT_NULL_MSG);
            }

            return await _verificationService.VerifyPINCodeResetPassword(model.PhoneNumber, model.PIN);
        }
    }
}
