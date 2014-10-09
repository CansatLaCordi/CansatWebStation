using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cansat.Setebos.Web.Models
{
    public class FlightData
    {
        public int Id { get; set; }
        public int FlightId { get; set; }

        public DateTime? DateTime { get; set; }
        public double? Altitude { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? CO { get; set; }
        public double? Presure { get; set; }
        public double? Humidity { get; set; }
        public double? Voltage { get; set; }
        public double? Temperature { get; set; }
        public double? InternalTemperature { get; set; }
    }
}