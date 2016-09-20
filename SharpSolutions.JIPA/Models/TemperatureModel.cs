using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Models
{
    public class TemperatureModel : ModelBase
    {
        public TemperatureModel()
        {
            
        }
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

        private string _Label;

        public string Label
        {
            get
            {
                return _Label;
            }
            set
            {
                if (_Label != value)
                {
                    _Label = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
