using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cansat.Station.Core
{
    class Measure
    {
        public string DeviceId { get; set; }
        public DateTime MeasureDate { get; set; }
        public float ExternalTemperature { get; set; }
        public float InternalTemperature { get; set; }
        public float Preasure { get; set; }
        public float Altitude { get; set; }
        public double Latitud { get; set; }
        public double Longitude { get; set; }
        public float Speed { get; set; }
        public float BarometricAltitude { get; set; }
        public float Humidity { get; set; }
        public float PM10 { get; set; }
        public bool Ejected { get; set; }
        public float BatteryVoltage { get; set; }

        public override string ToString()
        {
            return string.Format("[{1}]{0}: ", MeasureDate, DeviceId);
        }
    }
}
