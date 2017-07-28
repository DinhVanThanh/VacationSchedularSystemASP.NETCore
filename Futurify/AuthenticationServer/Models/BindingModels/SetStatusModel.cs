using JobHop.Common.Enums;
using JobHop.Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models.BindingModels
{
    public class SetStatusModel
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public AccountType Type { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public UserStatus Status { get; set; }
    }
}
