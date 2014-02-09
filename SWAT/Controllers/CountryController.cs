using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SWAT.Models;

namespace SWAT.Controllers
{
    public class CountryController : Controller
    {
        private SWATEntities db = new SWATEntities();

        // GET: /Country/
        public ActionResult Index()
        {
            var lkpcountries = db.lkpCountries.Include(l => l.lkpRegion);
            return View(lkpcountries.ToList());
        }

        // GET: /Country/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lkpCountry lkpcountry = db.lkpCountries.Find(id);
            if (lkpcountry == null)
            {
                return HttpNotFound();
            }
            return View(lkpcountry);
        }

        // GET: /Country/Create
        public ActionResult Create()
        {
            ViewBag.RegionID = new SelectList(db.lkpRegions, "ID", "Name");
            return View();
        }

        // POST: /Country/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name,CurrencyID,RegionID,CommonName")] lkpCountry lkpcountry)
        {
            if (ModelState.IsValid)
            {
                db.lkpCountries.Add(lkpcountry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RegionID = new SelectList(db.lkpRegions, "ID", "Name", lkpcountry.RegionID);
            return View(lkpcountry);
        }

        // GET: /Country/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lkpCountry lkpcountry = db.lkpCountries.Find(id);
            if (lkpcountry == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegionID = new SelectList(db.lkpRegions, "ID", "Name", lkpcountry.RegionID);
            return View(lkpcountry);
        }

        // POST: /Country/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name,CurrencyID,RegionID,CommonName")] lkpCountry lkpcountry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lkpcountry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RegionID = new SelectList(db.lkpRegions, "ID", "Name", lkpcountry.RegionID);
            return View(lkpcountry);
        }

        // GET: /Country/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lkpCountry lkpcountry = db.lkpCountries.Find(id);
            if (lkpcountry == null)
            {
                return HttpNotFound();
            }
            return View(lkpcountry);
        }

        // POST: /Country/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            lkpCountry lkpcountry = db.lkpCountries.Find(id);
            db.lkpCountries.Remove(lkpcountry);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
