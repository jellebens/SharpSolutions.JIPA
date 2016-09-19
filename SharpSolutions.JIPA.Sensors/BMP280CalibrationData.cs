using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Sensors
{
    public class BMP280CalibrationData
    {
        public UInt16 T1 { get; set; }
        public Int16 T2 { get; set; }
        public Int16 T3 { get; set; }
        public UInt16 P1 { get; set; }
        public Int16 P2 { get; set; }
        public Int16 P3 { get; set; }
        public Int16 P4 { get; set; }
        public Int16 P5 { get; set; }
        public Int16 P6 { get; set; }
        public Int16 P7 { get; set; }
        public Int16 P8 { get; set; }
        public Int16 P9 { get; set; }
    }
}
