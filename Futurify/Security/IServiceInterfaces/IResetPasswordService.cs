using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.IServiceInterfaces
{
    public interface IResetPasswordService
    {
        Task SendResetPasswordCodeAndUrl(string email);
        Task ResetPassword(string email, string code, string password);
    }
}
