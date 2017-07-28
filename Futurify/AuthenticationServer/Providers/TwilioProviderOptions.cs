using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Providers
{
    public class TwilioProviderOptions
    {
        public string AccountSID { get; set; }
        public string AuthToken { get; set; }
        public string FromNumber { get; set; }
    }
}
