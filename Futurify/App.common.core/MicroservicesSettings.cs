using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.common.core
{
    class MicroservicesSettings
    {
        public MicroserviceSetting[] Services { get; set; }

        public MicroserviceSetting FindByName(string name)
        {
            return this.Services.FirstOrDefault(s => s.Name == name);
        }
    }
    public class MicroserviceSetting
    {
        public string Name { get; set; }
        public string EndPoint { get; set; }
    }
}
