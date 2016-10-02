using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Models
{
    public class MessageModel : ModelBase
    {
        private bool _Toggle;
        public bool Toggle
        {
            get
            {
                return _Toggle;
            }

            set
            {
                if (_Toggle != value) {
                    _Toggle = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
