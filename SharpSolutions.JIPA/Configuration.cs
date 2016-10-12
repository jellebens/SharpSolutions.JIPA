using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA
{
    public class Configuration
    {
        private static Configuration _Current;
        static Configuration()
        {
            _Current = Load();
        }
        public Configuration()
        {

        }

        public string DeviceId { get; set; }
        public string Room { get; set; }
        public string Site { get; set; }
        public string ClientId { get; set; }
        public string LocalBus { get; set; }

        public static Configuration Load()
        {

            string json = File.ReadAllText("Settings.json");

            return JsonConvert.DeserializeObject<Configuration>(json);
        }

        public static Configuration Current
        {
            get
            {
                return _Current;
            }
        }

        
    }
}
