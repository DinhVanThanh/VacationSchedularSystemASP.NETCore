using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models.ViewModels
{
    public class FacebookDebugTokenResponse
    {
        [JsonProperty("data")]
        public FacebookDebugTokenResponseData Data { get; set; }
    }

    public class FacebookDebugTokenResponseData
    {
        [JsonProperty("app_id")]
        public string AppId { get; set; }
        [JsonProperty("user_id")]
        public string FacebookId { get; set; }
    }
}
