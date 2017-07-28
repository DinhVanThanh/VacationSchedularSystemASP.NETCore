using System;
using System.Collections.Generic;
using System.Text;

namespace App.common.core
{
    public static class CommonFunctions
    {
        public static string GenerateVerificationCode(bool digitsOnly)
        {
            if (!digitsOnly)
            {
                return Guid.NewGuid().ToString("N");
            }
            else
            {
                return new Random().Next(10000, 1000000).ToString("D6");
            }
        }
        public static int ConvertToInt(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
            return Int32.Parse(value);
        }
    }
}
