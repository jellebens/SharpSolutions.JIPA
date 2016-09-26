using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Core
{
    public class Messages
    {
        public static string CreateServiceStartMsg(string serviceName, string deviceId) {
            return string.Format("{{Message: \"{0}\", DeviceId=\"{1}\" Key:\"{2}\" TimeStamp:\"{3}\" }}", serviceName, deviceId ,"ServiceStarted", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }

        public static string CreateServiceStopMsg(string serviceName, string deviceId)
        {
            return string.Format("{{Message: \"{0}\", DeviceId=\"{1}\" Key:\"{2}\" TimeStamp:\"{3}\" }}", serviceName, deviceId, "ServiceStopped", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }
    }
}
