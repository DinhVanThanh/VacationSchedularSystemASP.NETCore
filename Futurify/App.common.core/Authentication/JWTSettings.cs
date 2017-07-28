using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace App.common.core.Authentication
{
    public class JWTSettings
    {
        public string Path { get; set; } = "/token";
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(20);

        public SigningCredentials SigningCredentials { get; set; }
    }
}
