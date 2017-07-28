using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Helpers
{
    public class VerificationCodeHelpers
    {
        public static string GenerateVerificationCode(bool onlyDigits)
        {
            if (!onlyDigits)
            {
                return Guid.NewGuid().ToString("N");
            }
            else
            {
                return new Random().Next(10000, 1000000).ToString("D6");
            }
        }
    }
}
