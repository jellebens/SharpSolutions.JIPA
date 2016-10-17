using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Models
{
    public class PowerConsumptionModel:ModelBase
    {
        public PowerConsumptionModel()
        {
         
        }

        private string _Power;

        public string Power
        {
            get { return _Power; }
            set { SetField(ref _Power, value); }
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
                SetField(ref _Label, value);
            }
        }
    }
}
