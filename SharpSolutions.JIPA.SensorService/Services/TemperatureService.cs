﻿using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using SharpSolutions.JIPA.Core;
using SharpSolutions.JIPA.Core.Mqtt;
using SharpSolutions.JIPA.Events.Metering;
using SharpSolutions.JIPA.Sensors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using Windows.Foundation;
using Windows.Foundation.Diagnostics;
using Windows.System.Threading;

namespace SharpSolutions.JIPA.SensorService.Services
{
    public sealed class TemperatureService: IService, IDisposable
    {
        private IBMP280 _Sensor;
        private SemaphoreSlim _Semaphore;
        private ThreadPoolTimer _Timer;
        private MqttClient _Client;
        private LoggingChannel _LoggingChannel;
        
        private const string Topic = "bir57/sensors/temperature";

        public TemperatureService(LoggingChannel loggingChannel)
        {
            _Sensor = new BMP280();
            _Semaphore = new SemaphoreSlim(1);
            
            _LoggingChannel = loggingChannel;
        }
        
        public IAsyncAction Start() {
            _LoggingChannel.LogMessage("TemperatureService.Start()", LoggingLevel.Information);
            ///Crap construction for winrt WME1038
            return AsyncInfo.Run(async delegate (CancellationToken token)
            {
                _Client = MqttClientFactory.CreatePublisher(Configuration.Default.LocalBus, $"TemperatureService_{Configuration.Default.ClientId}");
                await _Sensor.Initialize();
                
                _Timer = ThreadPoolTimer.CreatePeriodicTimer(OnTimerElapsedHandler, TimeSpan.FromSeconds(5));
            });
            
            

        }

        private async void OnTimerElapsedHandler(ThreadPoolTimer timer)
        {
            if (!_Semaphore.Wait(0)) return; //if lock is being held exit immediately
            
            try {
                float temp = await _Sensor.ReadTemperature();

                MeteringMeasuredEvent evnt = new MeteringMeasuredEvent();
                evnt.Key = string.Format("{0}_temperature", Configuration.Default.DeviceId);
                evnt.Site = Configuration.Default.Site;
                evnt.Value = temp.ToString();
                evnt.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                string payload = JsonConvert.SerializeObject(evnt);

                byte[] msg =Encoding.UTF8.GetBytes(payload);

                try
                {
                    if (!_Client.IsConnected)
                    {
                        _LoggingChannel.LogMessage("Client is not connected. reconnecting", LoggingLevel.Information);

                        _Client.Reconnect();
                    }
                    _Client.Publish(Topic, msg);
                }
                catch (MqttCommunicationException exc)
                {
                    string errMsg = "Failed to sent message: " + exc.Message;
                    Debug.WriteLine(errMsg);
                    _LoggingChannel.LogMessage(errMsg, LoggingLevel.Critical);
                    _Client.Disconnect();
                }
                catch (Exception exc) {
                    string errMsg = "Failed to sent message: " + exc.Message;
                    Debug.WriteLine(errMsg);
                    _LoggingChannel.LogMessage(errMsg, LoggingLevel.Critical);
                }
            }finally {
                _Semaphore.Release();
            }
            
        }

        public void Dispose()
        {
            string msg = Messages.CreateServiceStopMsg("Jelle's Intelligent Personal Assistant's Sensors", Configuration.Default.DeviceId);

            _Client.Publish(Topics.GetJipaSystemTopic(), Encoding.UTF8.GetBytes(msg));

            _Timer.Cancel();

            _Client.Disconnect();

            _Semaphore.Dispose();
        }
    }
}
