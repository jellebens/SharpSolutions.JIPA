using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Models
{
    public class MessageModel : ModelBase
    {
        public MessageModel()
        {
            this.Label = '\xE871';
        }
        private char _Label;
        public char Label
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
