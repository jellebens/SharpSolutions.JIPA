using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Models
{
    public class TemperatureModel : ModelBase
    {
        private string _Temperature;

        public string Temperature
        {
            get
            {
                return _Temperature;
            }
            set
            {
                if (_Temperature != value)
                {
                    _Temperature = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _Pressure;

        public string Pressure
        {
            get
            {
                return _Pressure;
            }
            set
            {
                if (_Pressure != value)
                {
                    _Pressure = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
