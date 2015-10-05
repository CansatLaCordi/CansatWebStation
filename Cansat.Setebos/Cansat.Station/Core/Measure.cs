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
        public double? ExternalTemperature { get; set; }
        public double? InternalTemperature { get; set; }
        public double? Preasure { get; set; }
        public double? Altitude { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Speed { get; set; }
        public double? BarometricAltitude { get; set; }
        public double? Humidity { get; set; }
        public double? PM10 { get; set; }
        public bool? Ejected { get; set; }
        public double? BatteryVoltage { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] ExternalTemp:{1}, InternalTemp:{2}, Preasure:{3}, Altitude:{4}, Latitud:{5}, Longitude:{6}, Speed:{7}, BarometricAltitude:{8}, Humidity:{9}, PM10:{10}, Ejected:{11}, Battery:{12}",
                MeasureDate,
                ExternalTemperature,
                InternalTemperature,
                Preasure,
                Altitude,
                Latitude,
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
