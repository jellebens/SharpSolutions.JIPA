using System.Threading.Tasks;
using Windows.Foundation;

namespace SharpSolutions.JIPA.Sensors
{
    public interface IBMP280
    {
        IAsyncAction Initialize();
        IAsyncOperation<float> ReadPreasure();
        IAsyncOperation<float> ReadTemperature();
    }
}