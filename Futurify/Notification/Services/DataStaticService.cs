using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using App.common.core.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Notification.IServiceInterfaces;
using Notification.Models;

namespace Notification.Services
{
    public class DataStaticService : IDataStaticService
    {
        private readonly IHostingEnvironment _env;
        public DataStaticService(IHostingEnvironment env)
        {
            _env = env;
        }

        public MailTemplate GetMailTemplate(string code)
        {
            try
            {
                string pathXMLFile = "\\Data\\EmailTemplates\\" + code + ".xml";
                XmlSerializer serTemplate = new XmlSerializer(typeof(ContainerMailTemplate));
                using (FileStream fsMailTemplate = new FileStream(_env.ContentRootPath + pathXMLFile, FileMode.Open))
                {
                    MailTemplate[] container = ((ContainerMailTemplate)serTemplate.Deserialize(fsMailTemplate)).MailTemplates;
                    return container.FirstOrDefault();
                }
            }
            catch(Exception e)
            {
                throw new CustomException("Errors.ERROR_GET_EMAIL_TEMPLATE");
            }
        }
    }
}
