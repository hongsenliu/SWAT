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
    public class RegionController : Controller
    {
        private SWATEntities db = new SWATEntities();

        // GET: /Region/
        public ActionResult Index()
        {
            return View(db.lkpRegions.ToList());
        }

        // GET: /Region/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lkpRegion lkpregion = db.lkpRegions.Find(id);
            if (lkpregion == null)
            {
                return HttpNotFound();
            }
            return View(lkpregion);
        }

        // GET: /Region/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Region/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name")] lkpRegion lkpregion)
        {
            if (ModelState.IsValid)
            {
                db.lkpRegions.Add(lkpregion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lkpregion);
        }

        // GET: /Region/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lkpRegion lkpregion = db.lkpRegions.Find(id);
            if (lkpregion == null)
            {
                return HttpNotFound();
            }
            return View(lkpregion);
        }

        // POST: /Region/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name")] lkpRegion lkpregion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lkpregion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lkpregion);
        }

        // GET: /Region/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lkpRegion lkpregion = db.lkpRegions.Find(id);
            if (lkpregion == null)
            {
                return HttpNotFound();
            }
            return View(lkpregion);
        }

        // POST: /Region/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            lkpRegion lkpregion = db.lkpRegions.Find(id);
            db.lkpRegions.Remove(lkpregion);
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
