using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cansat.Setebos.Data;
using System.Diagnostics;

namespace Cansat.PruebaSerial
{
    public class SerialParser
    {
        public CansatEntities Db { get; set; }

        public Flights Flight { get; set; }

        public SerialParser() {
            Db = new CansatEntities();
            Flight = Db.Flights.Where(f => f.Active).FirstOrDefault();
        }

        public void InsertLine(string measureLine) {
            Trace.WriteLine(measureLine,"[Data]");
            Dictionary<string, string> measures = new Dictionary<string, string>();
            foreach (var m in measureLine.Split('&')) {
                measures[m.Split('=').First()] = m.Split('=').Last();
            }
            try
            {
                Data d = new Data();
                d.Presure = float.Parse(measures["Pres"]);
                d.Temperature = float.Parse(measures["Temp"]);
                d.Altitude = float.Parse(measures["Alt"]);
                d.Latitude = float.Parse(measures["Latitud"]);
                d.Longitude = float.Parse(measures["Longitud"]);
                //d.Humidity = float.Parse(measures["Humedad"]);
                d.Datetime = DateTime.Now;
                d.Flights = this.Flight;
                Db.Data.Add(d);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);                
            }
            


        }
    }
}
