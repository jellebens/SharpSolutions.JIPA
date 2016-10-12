using SharpSolutions.JIPA.Core;
using SharpSolutions.JIPA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.ViewModels
{
    public class TimeViewModel
    {
        public TimeViewModel()
        {
            this.Model = new TimeModel();
            this.Update();
        }

        public void Update() {
            DateTimeOffset now = DateTimeOffsetProvider.Now;
            this.Model.Weekday = now.ToString("ddd");
            this.Model.Day = now.ToString("dd");

            string hourFormatString = (now.Second % 2) == 0? "HH:mm" : "HH mm";

            this.Model.Time = now.ToString(hourFormatString);

        }

        public TimeModel Model { get; private set; }
    }
}
