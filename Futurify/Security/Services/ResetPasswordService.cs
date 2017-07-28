using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.common.core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RawRabbit;
using Security.Helpers;
using Security.IServiceInterfaces;
using Security.Models;
using Security.Options;
using Vacation.common;
using Vacation.common.Events;

namespace Security.Services
{
    public class ResetPasswordService : IResetPasswordService
    {
        private IBusClient _rawRabbitBus;
        private AuthContext _context;
        private readonly ResetPasswordOptions _resetPasswordOptions;
        public ResetPasswordService(IOptions<ResetPasswordOptions> resetPasswordOptions, AuthContext context, IBusClient rawRabbitBus)
        {
            _rawRabbitBus = rawRabbitBus;
            _context = context;
            _resetPasswordOptions = resetPasswordOptions.Value;
        }
        public Task ResetPassword(string email, string code, string password)
        {
            throw new NotImplementedException();
        }

        public async Task SendResetPasswordCodeAndUrl(string email)
        {
            try
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserName.ToLower() == email.ToLower());

                if (account == null)
                {
                    throw new CustomException("Errors.EMAIL_NOT_FOUND", "Errors.EMAIL_NOT_FOUND");
                }

                var verification = await _context.VerificationCodes.FirstOrDefaultAsync(v => v.AccountId == account.AccountId && !v.Used && v.ExpiredAt > DateTime.Now && v.Purpose == VerificationPurpose.ResetPassword);

                if (verification == null)
                {
                    verification = new VerificationCode
                    {
                        AccountId = account.AccountId,
                        CheckFailedCounter = 0,
                        Code = VerificationCodeHelpers.GenerateVerificationCode(false),
                        CreatedAt = DateTime.Now,
                        ExpiredAt = DateTime.Now.AddMinutes(_resetPasswordOptions.EXPIRES_IN_MINUTES),
                        CodeReceiver = email,
                        Purpose = VerificationPurpose.ResetPassword,
                        ReceiverType = VerificationReceiverType.Email,
                        SendCounter = 1,
                        Used = false
                    };

                    _context.VerificationCodes.Add(verification);
                }
                else
                {
                    if (verification.SendCounter >= _resetPasswordOptions.MAX_SEND)
                    {
                        throw new CustomException("Errors.RESEND_RECOVERY_CODE_TOO_MUCH", verification.ExpiredAt.ToString("o"));
                    }
                    else if (verification.CheckFailedCounter >= _resetPasswordOptions.MAX_CHECKED_FAILED)
                    {
                        throw new CustomException("Errors.TRY_CODE_TOO_MUCH", verification.ExpiredAt.ToString("o"));
                    }

                    verification.SendCounter++;
                }

                await _context.SaveChangesAsync();
                await _rawRabbitBus.PublishAsync<RequestForgotPasswordForEmail>(new RequestForgotPasswordForEmail { Code = verification.Code, Email = email, ChangePasswordUrl = CommonContants.ForgotPasswordUrl });
                //await _rawRabbitBus.PublishAsync(new ResetPasswordCodeGenerated
                //{
                //    AccountId = verification.AccountId,
                //    Email = verification.CodeReceiver,
                //    Code = verification.Code
                //});
            }
            catch(Exception ex)
            {
                throw new CustomException(ex.Message, ex.InnerException.Message);
            }
        }

    }
}
