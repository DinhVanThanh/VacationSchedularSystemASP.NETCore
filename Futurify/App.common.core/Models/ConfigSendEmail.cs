using System;
using System.Collections.Generic;
using System.Text;

namespace App.common.core.Models
{
    public class ConfigSendEmail
    {
        public string Sender { get; set; }
        public List<string> Receivers { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }

        public ConfigSendEmail()
        {
            this.Receivers = new List<string>();
        }

        public ConfigSendEmail(ConfigSendEmail config)
        {
            this.Host = config.Host;
            this.Password = config.Password;
            this.Port = config.Port;
            this.Sender = config.Sender;
            this.Username = config.Username;
            this.Receivers = new List<string>();
        }
    }
}
