using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Contracts
{
    public class TemperatureChangedEventArgs: EventArgs
    {
        public string Key { get; set; }
        public float Value { get; set; }
        public string Unit { get; set; }
        public string Label { get; set; }
    }
}
