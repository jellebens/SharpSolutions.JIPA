﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Events.Metering
{
    public class MotionDetectedEvent
    {
        public string Key { get; set; }
        public string Site { get; set; }
        public long Timestamp { get; set; }
    }
}
