using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Models
{
    public class PowerConsumptionModel: ModelBase
    {
        private float _Value;
        public float Value {
            get {
                return _Value;
            }

            set {
                SetField(ref _Value,value);
            }
        }

        private string _Name;
        public string Name {
        get {
                return _Name;
            }
            set {
                SetField(ref _Name, value);
            }
        }
    }
}
