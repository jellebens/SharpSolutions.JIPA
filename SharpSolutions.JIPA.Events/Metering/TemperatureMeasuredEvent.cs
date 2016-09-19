using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Events.Metering
{
    public class TemperatureMeasuredEvent
    {
        public string Room { get; set; }
        public string Site { get; set; }
        public float Temperature { get; set; }
    }
}
