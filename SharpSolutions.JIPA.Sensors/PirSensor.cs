using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace SharpSolutions.JIPA.Sensors
{
    /// <summary>
    /// https://github.com/ms-iot/pir-sensor/blob/develop/PirSensor/PirSensor.cs
    /// </summary>
    public class PirSensor: IDisposable
    {
        /// <summary>
        /// Sensor type: Active high or active low.
        /// </summary>
        public enum SensorType
        {
            ActiveHigh, // Sensor pin is pulled high when motion is detected 
            ActiveLow   // Sensor pin is pulled low when motion is detected 
        }

        private GpioPin pirSensorPin;

        // <summary>
        /// The edge to compare the signal with for motion based on the sensor type.
        /// </summary>
        private GpioPinEdge pirSensorEdge;

        public event EventHandler<GpioPinValueChangedEventArgs> motionDetected;

        /// <summary>
        /// Constructs a motion sensor device.
        /// </summary>
        /// <param name="sensorPin">
        /// The GPIO number of the pin used for the motion sensor.
        /// </param>
        /// <param name="sensorType">
        /// The motion sensor type: Active low or active high
        /// </param>
        public PirSensor(int sensorPin, SensorType sensorType)
        {
            var gpioController = GpioController.GetDefault();
            if (gpioController != null)
            {
                pirSensorEdge = sensorType == SensorType.ActiveLow ? GpioPinEdge.FallingEdge : GpioPinEdge.RisingEdge;
                pirSensorPin = gpioController.OpenPin(sensorPin);
                pirSensorPin.SetDriveMode(GpioPinDriveMode.Input);
                pirSensorPin.ValueChanged += OnPirSensorPinValueChanged;
            }
            else
            {
                Debug.WriteLine("Error: GPIO controller not found.");
            }
        }

        /// <summary>
        /// Occurs when motion sensor pin value has changed
        /// </summary>
        private void OnPirSensorPinValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (motionDetected != null && args.Edge == pirSensorEdge)
            {
                motionDetected(this, args);
            }
        }

        public void Dispose()
        {
            pirSensorPin.Dispose();
        }
    }
}
