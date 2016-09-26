using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.EventService
{
    public sealed class Configuration
    {
        private static readonly Configuration _Current = Load();
        static Configuration()
        {
            
        }
        private Configuration()
        {

        }

        public string DeviceId { get; set; }
        public string DeviceKey { get; set; }
        public string IotHub { get; set; }
        
        public Topic[] Topics { get; set; }
        public string LocalBus { get; set; }

        public string ClientId { get; set; }

        public static Configuration Load()
        {

            string json = File.ReadAllText("Settings.json");

            return JsonConvert.DeserializeObject<Configuration>(json);
        }

        public static Configuration Default
        {
            get
            {
                return _Current;
            }
        }

    }
}
