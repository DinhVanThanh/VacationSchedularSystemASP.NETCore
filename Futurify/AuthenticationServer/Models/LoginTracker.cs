using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models
{
    public class LoginTracker
    {
        public int Id { get; set; }
        public DateTime LoginAt { get; set; }
        public Account Account { get; set; }
    }
}
