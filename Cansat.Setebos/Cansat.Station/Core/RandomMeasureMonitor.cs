using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cansat.Station.Core
{
    public class RandomMeasureMonitor : IMeasureMonitor
    {
        bool listening;
        Thread monitorThread ;
        public string DeviceId { get; set; }
        public bool Listening
        {
            get
            {
                return listening;
            }

            private set
            {
                listening = value;
            }
        }

        public event EventHandler<MeasureEventArgs> MeasureReceived;

        public RandomMeasureMonitor()
        {
            monitorThread = new Thread(new ThreadStart(Listen));
        }
        public RandomMeasureMonitor(string deviceId)
        {
            DeviceId = deviceId;
           
        }

        public void StartListening()
        {
            Listening = true;
            monitorThread = new Thread(new ThreadStart(Listen));
            monitorThread.Start();
        }

        public void EndListening()
        {
            Listening = false;
        }

        protected virtual void OnMeasureReceived(Measure measure)
        {
            if (MeasureReceived != null)
            {
                var e = new MeasureEventArgs(measure);
                MeasureReceived(this, e);
            }
        }

        void Listen()
        {
            Random rdn = new Random();
            while (Listening)
            {
                Measure m = new Measure()
                {
                    MeasureDate = DateTime.Now,
                    Altitude = 1500 + rdn.Next(50),
                    BarometricAltitude = 1500 + rdn.Next(50),
                    BatteryVoltage = 4.5f + (float)rdn.NextDouble(),
                    DeviceId = this.DeviceId,
                    Ejected = rdn.Next(2) == 1,
                    ExternalTemperature = 21f + (float)rdn.NextDouble() * 2f,
                    Humidity = 60f + (float)rdn.NextDouble() * 10f,
                    InternalTemperature = 21f + (float)rdn.NextDouble() * 2f,


                };

                OnMeasureReceived(m);
                Thread.Sleep(1000);
            }
        }
    }
}
