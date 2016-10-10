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
                if (_Value != value) {
                    _Value = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _Name;
        public string Name {
        get {
                return _Name;
            }
            set {
                if (_Name != value) {
                    _Name = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
