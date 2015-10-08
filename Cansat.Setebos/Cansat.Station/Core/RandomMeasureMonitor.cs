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

        public string SerialPortName { get; set; }
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
            monitorThread.SetApartmentState(ApartmentState.STA);
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
                    Voltage = 4.5f + rdn.NextDouble(),
                    DeviceId = this.DeviceId,
                    Ejected = rdn.Next(2) == 1,
                    ExternalTemperature = 21f + rdn.NextDouble() * 2f,
                    Humidity = 60f + rdn.NextDouble() * 10f,
                    InternalTemperature = 21f + rdn.NextDouble() * 2f,
                    Latitude = 20.5713651 + rdn.NextDouble() * 5,
                    Longitude = -103.6362335 + rdn.NextDouble() * 5,
                    Pressure = 100 + rdn.NextDouble() * 10,
                    Speed = 10 + rdn.NextDouble() * 10,
                    PM10= rdn.NextDouble()
                   
                };

                OnMeasureReceived(m);
                Thread.Sleep(1000);
            }
        }
    }
}
