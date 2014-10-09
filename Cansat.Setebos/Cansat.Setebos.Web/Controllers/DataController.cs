using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cansat.Setebos.Data;

namespace Cansat.Setebos.Web.Controllers
{
    public class DataController : Controller, Cansat.Setebos.Web.Controllers.IFlexigridController
    {
        private CansatEntities db = new CansatEntities();

        //
        // GET: /Data/

        public ActionResult Index()
        {
            var data = db.Data.Include(d => d.Flights);
            return View(data.ToList());
        }

        //
        // GET: /Data/Details/5

        public ActionResult Details(int id = 0)
        {
            Data.Data data = db.Data.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        //
        // GET: /Data/Create

        public ActionResult Create()
        {
            ViewBag.FlightId = new SelectList(db.Flights, "FlightId", "Name");
            return View();
        }

        //
        // POST: /Data/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Data.Data data)
        {
            if (ModelState.IsValid)
            {
                db.Data.Add(data);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FlightId = new SelectList(db.Flights, "FlightId", "Name", data.FlightId);
            return View(data);
        }

        //
        // GET: /Data/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Data.Data data = db.Data.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }
            ViewBag.FlightId = new SelectList(db.Flights, "FlightId", "Name", data.FlightId);
            return View(data);
        }

        //
        // POST: /Data/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Data.Data data)
        {
            if (ModelState.IsValid)
            {
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FlightId = new SelectList(db.Flights, "FlightId", "Name", data.FlightId);
            return View(data);
        }

        //
        // GET: /Data/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Data.Data data = db.Data.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        //
        // POST: /Data/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Data.Data data = db.Data.Find(id);
            db.Data.Remove(data);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult InsertRandom()
        {
            try
            {
                var activeFlights = db.Flights.Where(f => f.Active);
                Random rdm = new Random();
                foreach (var flight in activeFlights)
                {
                    var data = new Data.Data()
                    {
                        Altitude = 1400f + 100 * rdm.NextDouble(),
                        CO = rdm.NextDouble(),
                        Datetime = DateTime.Now,
                        Humidity = rdm.NextDouble(),
                        InternalTemperature = 30 + 10 * rdm.NextDouble(),
                        Latitude =  30 * rdm.NextDouble(),
                        Longitude = + 30 * rdm.NextDouble(),
                        Presure = 30 + 10 * rdm.NextDouble(),
                        Temperature = 30 + 10 * rdm.NextDouble(),
                        Voltage = 4 + 2 * rdm.NextDouble()
                    };
                    data.Flights = flight;
                    db.Data.Add(data);
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Content("Error: "+ex.StackTrace);
            }
            
            return Content("OK: "+DateTime.Now);
        }

        public ActionResult RandomData() {
            return View();
        }

        public ActionResult Flexigrid(Models.FlexigridModel model) {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            var data = db.Data.ToList().Skip(model.rp * (model.page - 1)).Take(model.rp);
            Models.FlexigridResponse<Data.Data, int> resp = new Models.FlexigridResponse<Data.Data, int>();
            resp.page = model.page;
            resp.total = db.Data.Count();
            resp.GenerateResponseRows(data.ToList(), d => d.DataId);
            return Json(resp);
               
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}