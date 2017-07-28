using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneNumbers;

namespace AuthenticationServer.Controllers
{
    [Route("api/Utils")]
    public class UtilsController : Controller
    {

        [HttpGet("is-valid-phone-number")]
        public bool IsValidPhoneNumber([FromQuery]string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return false;
            }
            else
            {
                try
                {
                    PhoneNumberHelpers.GetFormatedPhoneNumber(phoneNumber);
                    return true;
                }
                catch (NumberParseException)
                {
                    return false;
                }
            }
        }

    }
}