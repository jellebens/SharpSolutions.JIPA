using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Core.Mqtt
{
    public sealed class Topics
    {
        public static string Sensors = "bir57/sensors";
        public static string AllSensors = Sensors + "/#";
        public static string Temperature = Sensors + "/temperature";

        public static string GetJipaSystemTopic() {
            return "/jipa/system";
        }

        public static string GetOpenHabTopic()
        {
            return "/openhab";
        }
    }
}
