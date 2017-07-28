using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models.ViewModels
{
    public class FacebookVerifiedResponse
    {
        public bool IsValid { get; set; }
        public string FacebookId { get; set; }
    }
}
