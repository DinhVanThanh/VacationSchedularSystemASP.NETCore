using App.Common.Core.Exceptions;
using AuthenticationServer.Models;
using AuthenticationServer.Resources;
using AuthenticationServer.ServicesInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneNumbers;
using AuthenticationServer.Providers;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http;

namespace AuthenticationServer.Services
{
    public class ESMSService : IESMSService
    {
        private readonly ESMSProviderOptions _esmsOptions;
        public ESMSService(IOptions<ESMSProviderOptions> esmsOptions)
        {
            _esmsOptions = esmsOptions.Value;
        }

        public Task<HttpResponseMessage> SendSMS(string phone, string message, int smsType)
        {
            string url = _esmsOptions.Url + "?Phone=" + phone + "&Content=" + message + "&ApiKey=" + _esmsOptions.ApiKey+ "&SecretKey=" + _esmsOptions.SecrectKey + "&IsUnicode=0&SmsType="+smsType;
            var client = new HttpClient();
            var result =  client.GetAsync(url, HttpCompletionOption.ResponseContentRead);
            return result;
        }

    }
}
