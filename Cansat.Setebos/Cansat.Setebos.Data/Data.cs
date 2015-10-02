//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cansat.Setebos.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Data
    {
        public int DataId { get; set; }
        public int FlightId { get; set; }
        public Nullable<System.DateTime> Datetime { get; set; }
        public Nullable<double> Temperature { get; set; }
        public Nullable<double> Latitude { get; set; }
        public Nullable<double> Longitude { get; set; }
        public Nullable<double> Altitude { get; set; }
        public Nullable<double> Humidity { get; set; }
        public Nullable<double> Presure { get; set; }
        public Nullable<double> Voltage { get; set; }
        public Nullable<double> CO { get; set; }
        public Nullable<double> InternalTemperature { get; set; }
        public Nullable<double> PM10 { get; set; }
        public Nullable<double> Speed { get; set; }
        public Nullable<double> BarometricAltitude { get; set; }
        public Nullable<bool> Ejected { get; set; }
    
        public virtual Flights Flights { get; set; }
    }
}
