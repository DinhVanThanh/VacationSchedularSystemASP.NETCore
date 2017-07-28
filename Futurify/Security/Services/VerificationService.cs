using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.common.core.Exceptions;
using App.common.core.Helpers;
using Microsoft.EntityFrameworkCore;
using Security.IServiceInterfaces;
using Security.Models;

namespace Security.Services
{
    public class VerificationService : IVerificationService
    {
        private AuthContext _context;
        private IAccountService _accountService;

        public const int MAX_RETRY = 5;
        public const int MAX_RESEND = 5;

        public VerificationService(AuthContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }
        public async Task CreateAsync(VerificationCode verification)
        {
            _context.VerificationCodes.Add(verification);
            await _context.SaveChangesAsync();
        }

        public async Task VerifyRegistrationPhoneNumberAsync(string phoneNumber, string PIN)
        {
            //var verification = await this.GetVerificationCodeFromPhoneNumberAsync(phoneNumber, VerificationPurpose.RegistrationPhoneNumber);
            //if (verification == null)
            //{
            //    throw new CustomException("Errors.VERIFICATION_NOT_FOUND", "Errors.VERIFICATION_NOT_FOUND_MSG");
            //}
            //else if (verification. >= MAX_RETRY || verification.Resend > MAX_RESEND)
            //{
            //    throw new CustomException("Errors.VERIFICATION_LOCKED", "Errors.VERIFICATION_LOCKED_MSG");
            //}
            //else if (verification.ExpiredAt < DateTime.Now)
            //{
            //    throw new CustomException("Errors.VERIFICATION_EXPIRED", "Errors.VERIFICATION_EXPIRED_MSG");
            //}
            //else
            //{
            //    //checking PIN
            //    if (verification.VerifyCode != PIN)
            //    {
            //        //increase retry counter when failure
            //        verification.Retry++;
            //        if (verification.Retry >= MAX_RETRY)
            //        {
            //            //mark as expired if retry too much
            //            verification.ExpiredAt = DateTime.Now.AddMilliseconds(-1);
            //        }

            //        await _context.SaveChangesAsync();

            //        throw new CustomException("Errors.VERIFICATION_FAILED", verification.Retry.ToString());
            //    }
            //    else
            //    {
            //        verification.Checked = true;
            //        await _accountService.FindByIdAsync(verification.AccountId);

            //        verification.Account.PhoneNumber = verification.SetPhoneNumber;
            //        verification.Account.PhoneNumberVerified = true;

            //        await _context.SaveChangesAsync();
            //    }
            //}
        }

        public async Task<bool> VerifyByGuidAsync(string code)
        {
            //var verification = await _context.VerificationCodes.Include(s => s.Account).FirstOrDefaultAsync(c => c.Purpose != VerificationPurpose.RegistrationPhoneNumber && c.VerifyCode == code);
            //if (verification == null)
            //{
            //    return false;
            //}
            //else if (verification.ExpiredAt < DateTime.Now)
            //{
            //    _context.VerificationCodes.Remove(verification);
            //    await _context.SaveChangesAsync();
            //    return false;
            //}
            //else if ((bool)verification.Checked)
            //{
            //    return false;
            //}
            //else
            //{
            //    verification.Checked = true;

            //    switch (verification.Purpose)
            //    {
            //        case VerificationPurpose.Email:
            //            verification.Account.Email = verification.SetEmail;
            //            verification.Account.EmailVerified = true;
            //            break;
            //        case VerificationPurpose.Password:
            //            break;
            //        default:
            //            break;
            //    }

            //    await _context.SaveChangesAsync();
            //    return true;
            //}
            return true;
        }

        public async Task IncreaseResendCounter(int verificationId)
        {
            //var verification = await _context.VerificationCodes.FirstOrDefaultAsync(v => v.Id == verificationId);
            //verification.Resend++;
            //if (verification.Resend > MAX_RESEND)
            //{
            //    //mark verification as expired if re-send too much times
            //    verification.ExpiredAt = DateTime.Now.AddMilliseconds(-1);

            //    //TODO: should add the phone number to blacklisted in xxx minutes
            //}
            //else
            //{
            //    verification.ExpiredAt = DateTime.Now.AddDays(1);
            //}

            //await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Get verification for phone number that is not expired
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<VerificationCode> GetVerificationCodeFromPhoneNumberAsync(string phoneNumber, VerificationPurpose purpose)
        {
            //string formatedPhoneNumber = PhoneNumberHelpers.GetFormatedPhoneNumber(phoneNumber);
            return new VerificationCode();
            //return await _context.VerificationCodes.FirstOrDefaultAsync(a =>  a.SetPhoneNumber == formatedPhoneNumber && a.Purpose == purpose && !(bool)a.Checked);
        }

        public async Task<List<VerificationCode>> GetVerificationsOfAccount(int accountId)
        {
            return await _context.VerificationCodes.Where(v => v.AccountId == accountId).ToListAsync();
        }

        public async Task<bool> VerifyPINCodeResetPassword(string phoneNumber, string PIN)
        {
            //var verification = await GetVerificationCodeFromPhoneNumberAsync(phoneNumber, VerificationPurpose.Password);
            //if (verification == null || verification.VerifyCode != PIN)
            //    return false;

            //verification.Checked = true;
            //verification.ExpiredAt = DateTime.Now.AddMilliseconds(-1);

            //await _context.SaveChangesAsync();
            return true;
        }
    }
}
