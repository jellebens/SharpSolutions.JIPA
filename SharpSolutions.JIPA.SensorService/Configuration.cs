using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SharpSolutions.JIPA.SensorService
{
    public sealed class Configuration
    {
        public Configuration()
        {

        }

        public string DeviceId { get;  set; }
        public string DeviceKey { get;  set; }
        public string IotHub { get;  set; }
        public string Area { get;  set; }
        public int Interval { get;  set; }

        public static Configuration Load()
        {

            string json = File.ReadAllText("Settings.json");

            return JsonConvert.DeserializeObject<Configuration>(json);
        }
    }
}
