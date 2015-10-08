using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cansat.Station.Core
{
    public class Measure
    {
        public string DeviceId { get; set; }
        public DateTime? MeasureDate { get; set; }
        public double? ExternalTemperature { get; set; }
        public double? InternalTemperature { get; set; }
        public double? Pressure { get; set; }
        public double? Altitude { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Speed { get; set; }
        public double? BarometricAltitude { get; set; }
        public double? Humidity { get; set; }
        public double? PM10 { get; set; }
        public bool? Ejected { get; set; }
        public double? Voltage { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] ExternalTemp:{1:0.0}, InternalTemp:{2:0.0}, Pressure:{3:0.0}, Altitude:{4}, Latitud:{5}, Longitude:{6}, Speed:{7}, BarometricAltitude:{8}, Humidity:{9:0.0}, PM10:{10}, Ejected:{11}, Battery:{12:0.0}",
                MeasureDate,
                ExternalTemperature,
                InternalTemperature,
                Pressure,
                Altitude,
                Latitude,
                Longitude,
                Speed,
                BarometricAltitude,
                Humidity,
                PM10,
                Ejected,
                Voltage
                );
        }

        const string LinePattern = @"^\w+=.*(&\w=.*)*\r$";

        public static bool IsValidLine(string line)
        {
            //mdate=7-10-2015 4:14:0.0&extTemp=0.00&intTemp=0.00&press=0.00&lat=20.6295800&lon=-103.3026400&speed=0.17&barAlt=0.00&alt=1603.70&hum=-9077841800000000.00&pm10=0.00&volt=0.00
            Regex reg = new Regex(LinePattern);
            var r = reg.IsMatch(line);
            return r;

        }

        public static Measure ParseMeasure(string measureline)
        {
            //Example
            Dictionary<string, string> values = new Dictionary<string, string>();
            if (!IsValidLine(measureline)) return null;
            Measure m = new Measure();
            foreach (var item in measureline.Split('&'))
            {
                string key, value;
                try
                {
                    key = item.Split('=')[0];
                    value = item.Split('=')[1];
                    if (!string.IsNullOrEmpty(value) && value != "ovf")
                        switch (key)
                        {
                            case "mdate":

                                var datevals = value.Split(' ')[0].Split('-');
                                var timevals = value.Split(' ')[0].Split(':');
                                m.MeasureDate = new DateTime(
                                    int.Parse(datevals[2]),
                                    int.Parse(datevals[1]),
                                    int.Parse(datevals[0]),
                                    int.Parse(timevals[0]),
                                    int.Parse(timevals[1]),
                                    int.Parse(timevals[2].Split('.')[0]),
                                    int.Parse(timevals[2].Split('.')[1]),
                                    DateTimeKind.Utc);
                                break;
                            case "extTemp":
                                m.ExternalTemperature = double.Parse(value);
                                break;
                            case "intTemp":
                                m.InternalTemperature = double.Parse(value);
                                break;
                            case "lat":
                                m.Latitude = double.Parse(value);
                                break;
                            case "lon":
                                m.Longitude = double.Parse(value);
                                break;
                            case "vel":
                                m.Speed = double.Parse(value);
                                break;
                            case "barAlt":
                                m.BarometricAltitude = double.Parse(value);
                                break;
                            case "alt":
                                m.Altitude = double.Parse(value);
                                break;
                            case "hum":
                                m.Humidity = double.Parse(value);
                                break;
                            case "pm10":
                                m.Humidity = double.Parse(value);
                                break;
                            case "volt":
                                m.Voltage = double.Parse(value);
                                break;
                            case "pres":
                                m.Pressure = double.Parse(value);
                                break;
                            case "eje":
                                m.Ejected = int.Parse(value) == 1;
                                break;
                            default:
                                values[key] = value;
                                break;
                        }
                }
                catch (Exception)
                {

                    //throw;
                }
            }

            return m;
        }
    }
}
