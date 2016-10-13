using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Models
{
    public class MotionModel: ModelBase
    {
        private string _Label;

        public string Label
        {
            get { return _Label; }
            set { SetField(ref _Label, value); }
        }


        private int _Value;

        public int Value
        {
            get { return _Value; }
            set { SetField(ref _Value, value); }
        }

        internal void Decrease()
        {
            if (Value >= 1) {
                Value--;
            }
        }
    }
}
