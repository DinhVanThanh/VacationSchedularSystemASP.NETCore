using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneNumbers
{
    public class PhoneNumberHelpers
    {
        /// <summary>
        /// Format to international phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static string GetFormatedPhoneNumber(string phoneNumber)
        {
            PhoneNumber phone = null;
            PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
            var temp = phoneNumber;
            try
            {
                temp = "+" + phoneNumber.Trim('+');
                phone = phoneUtil.Parse(temp, "ZZ");
            }
            catch(Exception ex)//if cannot format, will format to
            {
                temp = string.Format("+84{0}", phoneNumber);
                phone = phoneUtil.Parse(temp, "ZZ");
            }

            return phoneUtil.Format(phone, PhoneNumberFormat.E164);
        }
    }
}
