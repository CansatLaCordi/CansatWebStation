using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cansat.Setebos.Data;

namespace LogParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"C:\Users\Francisco\Desktop\CansatWebStation\Cansat.Setebos\LogParser\Data.log";
            var lines = System.IO.File.ReadAllLines(path);
            Dictionary<string, string> meds = new Dictionary<string, string>();
            int parseid = 0;
            CansatEntities db = new Cansat.Setebos.Data.CansatEntities();
            parseid = (db.DataLog.Max(d => d.ParseId) ?? 0  )+ 1;
            DataLog dl;
            int cont=0;
            foreach (var line in lines)
            {
                try
                {
                    string l = line.Split(':')[1];
                    meds.Clear();
                    foreach (var medicion in l.Split('&'))
                    {
                        meds.Add(medicion.Split('=')[0], medicion.Split('=')[1]); 
                    }
                    dl = new DataLog();
                    dl.ParseId = parseid;
                    dl.Altitud = float.Parse(meds["Alt"]);
                    dl.GPSAltitud = float.Parse(meds["GPSAltitud"]);
                    dl.Latitud = float.Parse(meds["Latitud"]);
                    dl.Longitud = float.Parse(meds["Longitud"]);
                    dl.Presion = float.Parse(meds["Pres"]);
                    dl.PresionNivelMar = float.Parse(meds["PresMar"]);
                    dl.Satelites = float.Parse(meds["Satelites"]);
                    dl.Temperatura = float.Parse(meds["Temp"]);
                    dl.UTC = DateTime.Parse(meds["UTC"]);
                    dl.Velocidad = float.Parse(meds["Velociddad"]);
                    db.DataLog.Add(dl);
                    db.SaveChanges();
                    Console.WriteLine(string.Format("Registro numero {0} insertado", ++cont));                    
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message);                    
                }
            }


        }
    }
}
