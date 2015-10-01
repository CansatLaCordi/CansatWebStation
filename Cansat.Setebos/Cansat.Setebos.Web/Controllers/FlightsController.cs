using Cansat.Setebos.Data;
using Cansat.Setebos.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cansat.Setebos.Web.Controllers
{
    public class FlightsController : Controller
    {
        //
        // GET: /Flights/
        CansatEntities db = new CansatEntities();


        public ActionResult Index(int page = 1, int rowsPerPage = 10)
        {
            ViewData["page"] = page;
            ViewData["rowsPerPage"] = rowsPerPage;
            ViewData["count"] = db.Flights.Count();
            var result = db.Flights.OrderBy(f => f.FlightId).Skip((page - 1) * rowsPerPage).Take(rowsPerPage).ToList();
            return View(result);
        }

        public ActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Flights model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Flights.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var model = db.Flights.Find(id);
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(Flights model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry<Flights>(model).State = System.Data.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var model = db.Flights.Find(id);
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Delete(Flights model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry<Flights>(model).State = System.Data.EntityState.Deleted;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

            }
            return View(model);
        }

        public ActionResult ActualFlights()
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            var model = db.Flights.Where(f => f.Active).ToList();
            return Json(new { page = 1, total = model.Count, rows = model.Select(c => new { id = c.FlightId, cell = c }) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int id = -1)
        {
            if (id == -1)
                return new HttpNotFoundResult("Este vuelo no existe");
            var flight = db.Flights.Find(id);

            return View(flight);
        }

        public ActionResult GetFlightData(int id = -1)
        {
            //if (id == -1 || !db.Flights.Any(f => f.FlightId == id))
            //{
            //    return new HttpNotFoundResult();
            //}

            var flight = db.Flights.Find(id);
            var data = flight.Data.OrderBy(f => f.Datetime).Select(f => new FlightData()
            {
                Altitude = f.Altitude,
                DateTime = f.Datetime,
                CO = f.CO,
                FlightId = f.FlightId,
                Humidity = f.Humidity,
                Id = f.DataId,
                InternalTemperature = f.InternalTemperature,
                Latitude = f.Latitude,
                Longitude = f.Longitude,
                Presure = f.Presure,
                Temperature = f.Temperature,
                Voltage = f.Voltage
            }).Take(5000);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LastFlightData(int id = 0,int LastDataId = 0) {
            if (id == 0 ||LastDataId == 0 || !db.Flights.Any(f => f.FlightId == id))
            {
                return Json(new List<FlightData>(), JsonRequestBehavior.AllowGet);
            }

            var flight = db.Flights.Find(id);
            var data = flight.Data.OrderBy(f => f.Datetime).Where( f => f.DataId > LastDataId).Select(f => new FlightData()
            {
                Altitude = f.Altitude,
                DateTime = f.Datetime,
                CO = f.CO,
                FlightId = f.FlightId,
                Humidity = f.Humidity,
                Id = f.DataId,
                InternalTemperature = f.InternalTemperature,
                Latitude = f.Latitude,
                Longitude = f.Longitude,
                Presure = f.Presure,
                Temperature = f.Temperature,
                Voltage = f.Voltage
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Map(int id = -1) {
            var model = db.Flights.Find(id);
            return View(model);
        }
        public ActionResult GetLocations(int id = -1) {
            var locations = db.Data.Where(d => d.FlightId == id).OrderBy( d => d.Datetime).Select(d => new { d.Latitude, d.Longitude });
            return Json(locations);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
