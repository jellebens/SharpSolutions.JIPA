using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Models
{
    public class MessageModel : ModelBase
    {
        private static char[] labels = new[] { '\xE86C', '\xE86D', '\xE86E', '\xE86F', '\xE870', '\xE86F', '\xE86E', '\xE86D', '\xE86C' };
        private int _Counter = 0;
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
                SetField(ref _Label, value);
            }
        }

        public void UpdateLabel() {
            _Counter++;

            if (_Counter < 0 || _Counter >= labels.Length) {
                _Counter = 0;
            }
            
            Label = labels[_Counter];

        }
    }
}
