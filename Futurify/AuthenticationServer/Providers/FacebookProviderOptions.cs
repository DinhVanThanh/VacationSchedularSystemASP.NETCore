using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Providers
{
    public class FacebookProviderOptions
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string AppToken { get; set; }
    }
}
