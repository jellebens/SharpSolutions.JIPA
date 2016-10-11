using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Models
{
    public class TimeModel: ModelBase
    {
        private string _Date;
        public string Date
        {
            get
            {
                return _Date;
            }

            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Test { get; set; }

        private string _Time;
        public string Time
        {
            get
            {
                return _Time;
            }

            set
            {
                if (_Time != value)
                {
                    _Time = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
