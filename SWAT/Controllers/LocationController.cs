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
    public class LocationController : Controller
    {
        private SWATEntities db = new SWATEntities();

        // GET: /Location/
        public ActionResult Index()
        {
            var tblswatlocations = db.tblSWATLocations.Include(t => t.lkpCountry).Include(t => t.lkpRegion).Include(t => t.lkpSubnational);
            return View(tblswatlocations.ToList());
        }

        // GET: /Location/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATLocation tblswatlocation = db.tblSWATLocations.Find(id);
            if (tblswatlocation == null)
            {
                return HttpNotFound();
            }
            return View(tblswatlocation);
        }

        // GET: /Location/Create
        public ActionResult Create(int? uid)
        {
            ViewBag.countryID = new SelectList(db.lkpCountries, "ID", "Name");
            ViewBag.regionID = new SelectList(db.lkpRegions, "ID", "Name");
            ViewBag.subnationalID = new SelectList(db.lkpSubnationals, "ID", "Name");
            ViewBag.uid = uid;
            return View();
        }

        // POST: /Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,latitude,longitude,countryID,name,regionID,subnationalID")] tblSWATLocation tblswatlocation, int? uid)
        {
            if (ModelState.IsValid)
            {
                db.tblSWATLocations.Add(tblswatlocation);
                db.SaveChanges();
                //var LocationID = tblswatlocation.ID;
                
                return RedirectToAction("Create", "Survey", new { UserID = uid, LocationID = tblswatlocation.ID});
            }

            ViewBag.countryID = new SelectList(db.lkpCountries, "ID", "Name", tblswatlocation.countryID);
            ViewBag.regionID = new SelectList(db.lkpRegions, "ID", "Name", tblswatlocation.regionID);
            ViewBag.subnationalID = new SelectList(db.lkpSubnationals, "ID", "Name", tblswatlocation.subnationalID);
            return View(tblswatlocation);
        }

        // GET: /Location/Edit/5
        public ActionResult Edit(int? id, int? uid, int? SurveyID)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATLocation tblswatlocation = db.tblSWATLocations.Find(id);
            if (tblswatlocation == null)
            {
                return HttpNotFound();
            }

            ViewBag.countryID = new SelectList(db.lkpCountries, "ID", "Name", tblswatlocation.countryID);
            ViewBag.regionID = new SelectList(db.lkpRegions, "ID", "Name", tblswatlocation.regionID);
            ViewBag.subnationalID = new SelectList(db.lkpSubnationals, "ID", "Name", tblswatlocation.subnationalID);
            ViewBag.uid = uid;
            ViewBag.SurveyID = SurveyID;
            return View(tblswatlocation);
        }

        // POST: /Location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,latitude,longitude,countryID,name,regionID,subnationalID")] tblSWATLocation tblswatlocation, int? uid, int SurveyID)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatlocation).State = EntityState.Modified;
                db.SaveChanges();
                var BackgrdInfo = db.tblSWATBackgroundinfoes.Where(item => item.SurveyID == SurveyID);
                if (!BackgrdInfo.Any())
                {
                    tblSWATBackgroundinfo tblswatbackgroundinfo = new tblSWATBackgroundinfo();
                    tblswatbackgroundinfo.SurveyID = SurveyID;
                    db.tblSWATBackgroundinfoes.Add(tblswatbackgroundinfo);
                    db.SaveChanges();
                    var newBackgroundInfoID = tblswatbackgroundinfo.ID;
                    return RedirectToAction("Edit", "Background", new { id = newBackgroundInfoID, SurveyID = SurveyID });
                }
                else
                {
                    return RedirectToAction("Edit", "Background", new { id = BackgrdInfo.Select(item => item.ID).First(), SurveyID = SurveyID });
                }
                //return RedirectToAction("Details", "User", new { id=uid});
            }
            ViewBag.countryID = new SelectList(db.lkpCountries, "ID", "Name", tblswatlocation.countryID);
            ViewBag.regionID = new SelectList(db.lkpRegions, "ID", "Name", tblswatlocation.regionID);
            ViewBag.subnationalID = new SelectList(db.lkpSubnationals, "ID", "Name", tblswatlocation.subnationalID);
            return View(tblswatlocation);
        }

        // GET: /Location/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATLocation tblswatlocation = db.tblSWATLocations.Find(id);
            if (tblswatlocation == null)
            {
                return HttpNotFound();
            }
            return View(tblswatlocation);
        }

        // POST: /Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATLocation tblswatlocation = db.tblSWATLocations.Find(id);
            db.tblSWATLocations.Remove(tblswatlocation);
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
