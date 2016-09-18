using SharpSolutions.JIPA.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Service
{
    public class TemperatureService
    {
        private BMP280 _Sensor;
        private bool isSensorInitialized = false;

        public TemperatureService()
        {
            _Sensor = new BMP280();
            
        }

        public async Task InitSensor() {
            if (!isSensorInitialized) {
                await _Sensor.Initialize();
                isSensorInitialized = true;
            }
            
        }
        
        public async Task<float> ReadTemperature()
        {
            
            float temperature = await _Sensor.ReadTemperature();

            return temperature;
        }

        public async Task<float> ReadPreassure()
        {
            
            float temperature = await _Sensor.ReadPreasure();

            return temperature;
        }
    }
}
