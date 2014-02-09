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
    public class SubnationController : Controller
    {
        private SWATEntities db = new SWATEntities();

        // GET: /Subnation/
        public ActionResult Index()
        {
            var lkpsubnationals = db.lkpSubnationals.Include(l => l.lkpCountry);
            return View(lkpsubnationals.ToList());
        }

        // GET: /Subnation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lkpSubnational lkpsubnational = db.lkpSubnationals.Find(id);
            if (lkpsubnational == null)
            {
                return HttpNotFound();
            }
            return View(lkpsubnational);
        }

        // GET: /Subnation/Create
        public ActionResult Create()
        {
            ViewBag.CountryID = new SelectList(db.lkpCountries, "ID", "Name");
            return View();
        }

        // POST: /Subnation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,CountryID,Name")] lkpSubnational lkpsubnational)
        {
            if (ModelState.IsValid)
            {
                db.lkpSubnationals.Add(lkpsubnational);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CountryID = new SelectList(db.lkpCountries, "ID", "Name", lkpsubnational.CountryID);
            return View(lkpsubnational);
        }

        // GET: /Subnation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lkpSubnational lkpsubnational = db.lkpSubnationals.Find(id);
            if (lkpsubnational == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryID = new SelectList(db.lkpCountries, "ID", "Name", lkpsubnational.CountryID);
            return View(lkpsubnational);
        }

        // POST: /Subnation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,CountryID,Name")] lkpSubnational lkpsubnational)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lkpsubnational).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryID = new SelectList(db.lkpCountries, "ID", "Name", lkpsubnational.CountryID);
            return View(lkpsubnational);
        }

        // GET: /Subnation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lkpSubnational lkpsubnational = db.lkpSubnationals.Find(id);
            if (lkpsubnational == null)
            {
                return HttpNotFound();
            }
            return View(lkpsubnational);
        }

        // POST: /Subnation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            lkpSubnational lkpsubnational = db.lkpSubnationals.Find(id);
            db.lkpSubnationals.Remove(lkpsubnational);
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
