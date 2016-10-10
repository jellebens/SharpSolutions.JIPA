using Newtonsoft.Json;
using SharpSolutions.JIPA.Contracts.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Services
{
    public class SensorService
    {
        private Dictionary<string, Sensor> _Sensors;

        public static SensorService Create() {
            SensorService svc = new SensorService();
            svc._Sensors = new Dictionary<string, Sensor>();
            
            string all = File.ReadAllText("Data/sensor-list.json");

            Sensor[] sensors = JsonConvert.DeserializeObject<Sensor[]>(all);

            for (int i = 0; i < sensors.Length; i++)
            {
                svc._Sensors.Add(sensors[i].Key, sensors[i]);
            }

            return svc;
        }

        internal bool TryGet(string key, out Sensor s)
        {
            return _Sensors.TryGetValue(key, out s);
        }
    }
}
