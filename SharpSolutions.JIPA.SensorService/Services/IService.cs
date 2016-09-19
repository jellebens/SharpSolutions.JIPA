using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace SharpSolutions.JIPA.SensorService.Services
{
    public interface IService
    {
        IAsyncAction Start();
    }
}
