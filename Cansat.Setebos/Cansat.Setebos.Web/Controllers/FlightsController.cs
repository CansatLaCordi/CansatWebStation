using Cansat.Setebos.Data;
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

        public ActionResult Delete(int id) {
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
