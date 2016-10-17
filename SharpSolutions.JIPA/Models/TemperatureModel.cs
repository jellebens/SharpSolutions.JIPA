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
        private float _Temperature;

        public float Temperature
        {
            get
            {
                return _Temperature;
            }
            set
            {
                SetField(ref _Temperature, value);
            }
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

        private int _Max;

        public int Max
        {
            get { return _Max; }
            set { SetField(ref _Max, value); }
        }

        private int _Min;

        public int Min
        {
            get { return _Min; }
            set { SetField(ref _Min, value); }
        }

    }
}
