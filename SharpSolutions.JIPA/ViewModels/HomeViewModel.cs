using Amqp;
using ppatierno.AzureSBLite;
using ppatierno.AzureSBLite.Messaging;
using SharpSolutions.JIPA.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace SharpSolutions.JIPA.ViewModels
{
    public class HomeViewModel: ViewModelBase
    {

        

        public HomeViewModel()
        {
            Time = new TimeModel();
            Temperature = new TemperatureModel();

            Temperature.Temperature = "29.0 °C";
            Temperature.Pressure = "";

            string connectionString = string.Format("Endpoint={0};SharedAccessKeyName={1};SharedAccessKey={2}", Configuration.Current.EventHub, Configuration.Current.SharedAccessKeyName, Configuration.Current.SharedAccessKey);
            
            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(connectionString);
            
            EventHubClient client = factory.CreateEventHubClient("iothub-ehub-bitfrost-18320-269c8f9dc0");
            EventHubConsumerGroup group = client.GetDefaultConsumerGroup();
            
            EventHubReceiver receiver = group.CreateReceiver("1");

            while (true)
            {
                try
                {
                    EventData data = receiver.Receive();
                    if (data != null)
                    {
                        string f = UTF8Encoding.UTF8.GetString(data.GetBytes());


                        Debug.WriteLine(f);
                    }
                }
                catch (Exception e) {
                    Debug.WriteLine(e.Message);
                }finally
                {
                    Task.Delay(3000);
                }
                
                
            }

            receiver.Close();
            client.Close();
            factory.Close();



        }

        public TimeModel Time { get; private set; }
        public TemperatureModel Temperature { get; private set; }

        public void UpdateTime()
        {
            DateTime now = DateTime.Now;
            Time.Time = now.ToString("HH:mm:ss");
            Time.Date = now.ToString("ddd dd MMM yyy");
        }
        
    }
}
