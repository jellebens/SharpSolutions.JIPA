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

        private float _Power;

        public float Power
        {
            get { return _Power; }
            set { SetField(ref _Power, value); }
        }
        
        private string _Unit;

        public string Unit
        {
            get { return _Unit; }
            set { SetField(ref _Unit, value); }
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
