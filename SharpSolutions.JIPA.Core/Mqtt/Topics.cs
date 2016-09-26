using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Core.Mqtt
{
    public sealed class Topics
    {
        public static string GetJipaSystemTopic() {
            return "/jipa/system";
        }
    }
}
