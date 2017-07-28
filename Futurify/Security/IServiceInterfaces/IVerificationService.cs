using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Security.Models;

namespace Security.IServiceInterfaces
{
    public interface IVerificationService
    {
        Task CreateAsync(VerificationCode verification);
        Task<VerificationCode> GetVerificationCodeFromPhoneNumberAsync(string phoneNumber, VerificationPurpose purpose);
        Task<List<VerificationCode>> GetVerificationsOfAccount(int accountId);
        Task VerifyRegistrationPhoneNumberAsync(string phoneNumber, string PIN);
        Task<bool> VerifyByGuidAsync(string code);
        Task IncreaseResendCounter(int verificationId);
        Task<bool> VerifyPINCodeResetPassword(string phoneNumber, string PIN);
    }
}
