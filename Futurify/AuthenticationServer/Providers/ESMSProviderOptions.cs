using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Providers
{
    public class ESMSProviderOptions
    {
        public string ApiKey { get; set; }
        public string SecrectKey { get; set; }
        public string Url { get; set; }
    }
}
