using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cansat.Station.Core
{
    public class Measure
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
            return string.Format("[{0}] [{0}]: ExternalTemp:{2}, InternalTemp:{3}, Preasure:{4}, Altitude:{5}, Latitud:{6}, Longitude:{7}, Speed:{8}, BarometricAltitude:{9}, Humidity:{10}, PM10:{11}, Ejected:{12}, Battery:{13}",
                MeasureDate,
                DeviceId,
                ExternalTemperature,
                InternalTemperature,
                Preasure,
                Altitude,
                Latitud,
                Longitude,
                Speed,
                BarometricAltitude,
                Humidity, 
                PM10,
                Ejected, 
                BatteryVoltage
                );
        }
    }
}
