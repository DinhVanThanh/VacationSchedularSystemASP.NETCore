using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using App.common.core;
using App.common.core.Exceptions;
using App.common.core.Models;
using MailKit.Net.Smtp;
using MimeKit;
using Notification.IServiceInterfaces;
using Notification.Models;

namespace Notification.Services
{
    public class MailService : IMailService
    {
        public async Task SendMail<T>(ConfigSendEmail config, MailTemplate mailTemplate, T value)
        {
            try
            {
                var sender = config.Sender;
                var username = config.Username;
                var password = config.Password;
                var host = config.Host;
                var port =  config.Port.ConvertToInt();
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress( sender));
                emailMessage.To.AddRange(config.Receivers.Select(t => new MailboxAddress("", t)));
                mailTemplate = ReplaceHolderTemplate(mailTemplate, value);
                emailMessage.Subject = mailTemplate.Subject;
                mailTemplate.Body = mailTemplate.Body;
                emailMessage.Body = new TextPart("html") { Text = mailTemplate.Body };

                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync(host, port, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2"); // Must be removed for Gmail SMTP
                    await client.AuthenticateAsync(username, password);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch(Exception e)
            {
                throw new CustomException("Errors.ERROR_SEND_EMAIL");
            }
        }

        public MailTemplate ReplaceHolderTemplate<T>(MailTemplate template, T value)
        {
            try
            {
                MailTemplate currentTemplate = new MailTemplate();
                MergeObjects(template, currentTemplate);

                //assign basic value to return model

                // regEx to find replaceHolder
                string pattern = @"\" + "[" + @"\w+\" + "]";

                #region Replace properties of supporter
                var matchBody = Regex.Match(currentTemplate.Body, pattern);
                var matchSubject = Regex.Match(currentTemplate.Subject, pattern);
                //contain all placeholder of title and body
                List<string> placeHolders = new List<string>();

                //while has placeholder
                while (matchBody.Value != "")
                {
                    placeHolders.Add(matchBody.Value.Replace("[", "").Replace("]", ""));
                    matchBody = matchBody.NextMatch();
                }
                while (matchSubject.Value != "")
                {
                    placeHolders.Add(matchSubject.Value.Replace("[", "").Replace("]", ""));
                    matchSubject = matchSubject.NextMatch();
                }
                //distinct placeholders for optimized loop
                placeHolders = placeHolders.Distinct().ToList();

                //loop all received placeholders and replace it with real value
                string propertyName = null;
                foreach (var item in placeHolders)
                {
                    string replaceTo = "";
                    string placeHolder = "[" + item + "]";
                    try
                    {
                        propertyName = item;

                        dynamic propValue = GetValueOfProperty<T>(propertyName, value);


                        if (propValue is double || propValue is int || propValue is long)
                        {
                            replaceTo = String.Format("{0:### ### ### ###}", propValue);
                        }
                        else
                        {
                            replaceTo = propValue.ToString();
                        }
                    }
                    catch (Exception)
                    {
                        replaceTo = "";
                    }
                    currentTemplate.Body = currentTemplate.Body.Replace(placeHolder, replaceTo);
                    currentTemplate.Subject = currentTemplate.Subject.Replace(placeHolder, replaceTo);
                }
                #endregion
                return currentTemplate;
            }
            catch
            {
                throw new CustomException("Errors.ERROR_REPLACE_PLACEHOLDERS");
            }
        }

        /// <summary>
        /// Get value by property name
        /// Property Name can be a navigate to child property of navigation property
        /// Example: Assignee.Name. Assignee is name of Navigation Property of Workplan
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object GetValueOfProperty<T>(string propertyName, T value)
        {
            string[] navs = propertyName.Split('.');
            PropertyInfo[] props = typeof(T).GetProperties();
            var prop = props.FirstOrDefault(p => p.Name == navs[0]);
            if (navs.Length == 1)
            {
                return prop.GetValue(value);
            }
            else
            {
                var nextInject = navs.Skip(1).ToArray();
                var navVal = prop.GetValue(value);
                var type = navVal.GetType();
                MethodInfo method = typeof(MailService).GetMethod("GetValueOfProperty",
                    BindingFlags.Public | BindingFlags.Static);

                // Build a method with the specific type argument you're interested in
                method = method.MakeGenericMethod(type);

                object[] param = { string.Join(".", nextInject), navVal };

                object res = method.Invoke(null, param);
                return res;
            }
        }

        /// <summary>
        /// copy properties values from source to destination
        /// </summary>
        /// <param name="source">source object</param>
        /// <param name="destination">destination</param>
        public static void MergeObjects(object source, object destination)
        {
            var props = source.GetType().GetProperties();

            foreach (var prop in props)
            {
                //if (!Attribute.IsDefined(prop, typeof(ForeignKeyAttribute)))
                //{
                if (prop.Name != "Id" && prop.Name != "CreatedOn" && prop.CanWrite)
                {
                    prop.SetValue(destination, prop.GetValue(source));
                }
                //}
            }
        }
    }
}
