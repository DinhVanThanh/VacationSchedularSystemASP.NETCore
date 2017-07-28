using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Options
{
    public class ResetPasswordOptions
    {
        public int EXPIRES_IN_MINUTES { get; set; }
        public int MAX_SEND { get; set; }
        public int MAX_CHECKED_FAILED { get; set; }
    }
}
