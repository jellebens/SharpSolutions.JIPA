using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Models
{
    public class TimeModel: ModelBase
    {
        private string _Weekday;
        public string Weekday
        {
            get { return _Weekday; } 
            set { SetField(ref _Weekday, value); }
        }

        private string _Day;
        public string Day
        {
            get { return _Day; }
            set { SetField(ref _Day, value); }
        }

        private string _Time;
        public string Time
        {
            get{ return _Time; }
            set{ SetField(ref _Time, value); }
        }
    }
}
